using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Lib;

namespace NeuroNet
{
    public class Neuro
    {
        public bool stop = false;
        public delegate void Detect(List<Item> items);
        public event Detect OnDetect;
        private string url;
        public Neuro(string url)
        {
            this.url = url;
        }

        public void Run()
        {
            var net = DnnInvoke.ReadNetFromDarknet("./detection/yolov3.cfg", "./detection/yolov3.weights");
            var classLabels = File.ReadAllLines("./detection/coco.names");

            net.SetPreferableBackend(Emgu.CV.Dnn.Backend.OpenCV);
            net.SetPreferableTarget(Emgu.CV.Dnn.Target.Cpu);

            var vc = new VideoCapture(url);

            Mat frame = new Mat();
            VectorOfMat output = new VectorOfMat();

            VectorOfRect boxes = new VectorOfRect();
            VectorOfFloat scores = new VectorOfFloat();
            VectorOfInt indices = new VectorOfInt();

            while (true)
            {
                vc.Read(frame);

                CvInvoke.Resize(frame, frame, new System.Drawing.Size(0, 0), .4, .4);

                boxes = new VectorOfRect();
                indices = new VectorOfInt();
                scores = new VectorOfFloat();

                var image = frame.ToImage<Bgr, byte>();

                var input = DnnInvoke.BlobFromImage(image, 1 / 255.0, swapRB: true);

                net.SetInput(input);

                net.Forward(output, net.UnconnectedOutLayersNames);

                for (int i = 0; i < output.Size; i++)
                {
                    var mat = output[i];
                    var data = (float[,])mat.GetData();

                    for (int j = 0; j < data.GetLength(0); j++)
                    {
                        float[] row = Enumerable.Range(0, data.GetLength(1))
                                      .Select(x => data[j, x])
                                      .ToArray();

                        var rowScore = row.Skip(5).ToArray();
                        var classId = rowScore.ToList().IndexOf(rowScore.Max());
                        var confidence = rowScore[classId];

                        if (confidence > 0.8f)
                        {
                            var centerX = (int)(row[0] * frame.Width);
                            var centerY = (int)(row[1] * frame.Height);
                            var boxWidth = (int)(row[2] * frame.Width);
                            var boxHeight = (int)(row[3] * frame.Height);

                            var x = (int)(centerX - (boxWidth / 2));
                            var y = (int)(centerY - (boxHeight / 2));

                            boxes.Push(new System.Drawing.Rectangle[] { new System.Drawing.Rectangle(x, y, boxWidth, boxHeight) });
                            indices.Push(new int[] { classId });
                            scores.Push(new float[] { confidence });
                        }

                    }

                }

                List<Item> items = new List<Item>();

                var bestIndex = DnnInvoke.NMSBoxes(boxes.ToArray(), scores.ToArray(), .8f, .8f);

                for (int i = 0; i < bestIndex.Length; i++)
                {
                    int index = bestIndex[i];
                    var box = boxes[index];

                    int x = box.Width / 2 + box.X;
                    int y = box.Height / 2 + box.Y;
                    int id = indices[index];
                    items.Add(new Item
                    {
                        id = id,
                        x = x,
                        y = y,
                        name = classLabels[id]
                    });
                }

                OnDetect?.Invoke(items);

                if (stop)
                    break;

            }
        }
    }
}