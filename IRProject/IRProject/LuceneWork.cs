using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace IRProject
{
    class LuceneWork
    {
         private Analyzer analyzer = new WhitespaceAnalyzer(); 
        private Directory luceneIndexDirectory;
        private IndexWriter writer;
        private string indexPath = @"C:\lucene";

        public LuceneWork()
        {
            InitialiseLucene();
        }

        private void InitialiseLucene()
        {
            if(System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.Delete(indexPath,true);
            }

            luceneIndexDirectory =FSDirectory.Open(indexPath);
            writer = new IndexWriter(luceneIndexDirectory,analyzer,IndexWriter.MaxFieldLength.LIMITED);
        }

        public void BuildIndex(List<DocStructure> dataToIndex)
        {
            foreach (var sampleDataFileRow in dataToIndex)
	        {
		        Document doc = new Document();
                Console.Write("kek");
                doc.Add(new Field("mark", sampleDataFileRow.name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("review", sampleDataFileRow.review, Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(doc);
	        }
            writer.Optimize();
            //writer.Flush(true,true,true);
            writer.Dispose();
            //luceneIndexDirectory.Dispose();
        }

        public List<DocStructure> Search(string searchTerm)
        {
            IndexSearcher searcher = new IndexSearcher(luceneIndexDirectory);
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30,"LineText", analyzer);

            Query query = parser.Parse(searchTerm);
            TopScoreDocCollector cllctr = TopScoreDocCollector.Create(100, true);
            searcher.Search(query, cllctr); 
            ScoreDoc[] hits = cllctr.TopDocs().ScoreDocs;

            List<DocStructure> results = new List<DocStructure>();
            DocStructure sampleDataFileRow = null;
          
            for (int i = 0; i < hits.Length; i++)
            {
                sampleDataFileRow = new DocStructure();
                int docId = hits[i].Doc;
                float score = hits[i].Score;
                Document doc = searcher.Doc(docId);
                sampleDataFileRow.name =doc.Get("mark");
                sampleDataFileRow.review = doc.Get("review");
                //sampleDataFileRow.Score = score;

                results.Add(sampleDataFileRow);
            }
           
            return results;
        }
    }
}
