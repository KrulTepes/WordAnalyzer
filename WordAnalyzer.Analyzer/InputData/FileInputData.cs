using System;
using System.Collections;
using System.IO;

namespace WordAnalyzer.Analyzer.InputData
{
    public class FileInputData : IInputData
    {
        private readonly string _path;

        public FileInputData(string path)
        {
            _path = path;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            CheckExist();
            using StreamReader sr = File.OpenText(_path);
            while (sr.Peek() != -1)
            {
                yield return (char)sr.Read();
            }

            yield break;
        }

        public Stream GetStream()
        {
            CheckExist();
            StreamReader sr = File.OpenText(_path);
            return sr.BaseStream;
        }

        private void CheckExist()
        {
            if (!File.Exists(_path))
            {
                throw new Exception($"File '{_path}' not exists.");
            }
        }  
    }
}
