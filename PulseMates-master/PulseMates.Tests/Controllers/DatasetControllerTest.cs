namespace PulseMates.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PulseMates.Controllers;
    using PulseMates.Models;
    using PulseMates.Models.Storage;
    using System.Web.Http;
    using System.Net;

    class MockDatasetRepository : IDataSourceRepository
    {
        #region Field Members

        public static List<DataSource> Context = new List<DataSource>()
        {
            new DataSource { Id = "1", Name = "Test1", Description = "muuha" },
            new DataSource { Id = "2", Name = "Test2", Description = "muuha" },
            new DataSource { Id = "3", Name = "Test3", Description = "muuha" },
        };

        #endregion

        #region IDatasetRepository Members

        public DataSource Find(string id)
        {
            return Context.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<DataSource> FindAll()
        {
            return Context;
        }

        public DataSource Create(DataSource dataset)
        {
            var id = Context.Select(x => int.Parse(x.Id)).Max() + 1;
            dataset.Id = id.ToString();
            Context.Add(dataset);

            return dataset;
        }

        public DataSource Update(DataSource dataset)
        {
            var index = Context.FindIndex(x => x.Id == dataset.Id);
            Context[index] = dataset;

            return dataset;
        }

        public bool Delete(string id)
        {
            var index = Context.FindIndex(x => x.Id == id);
            Context.RemoveAt(index);

            return true;
        }

        #endregion

        public DataSourceValue FindItem(string dataSourceId, string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataSourceValue> FindAllItems(string dataSourceId)
        {
            throw new NotImplementedException();
        }

        public DataSourceValue Create(string dataSourceId, DataSourceValue dataSourceItem)
        {
            throw new NotImplementedException();
        }

        public DataSourceValue Update(string dataSourceId, DataSourceValue dataSourceItem)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string dataSourceId, string id)
        {
            throw new NotImplementedException();
        }
    }


    [TestClass]
    public class DatasetControllerTest
    {
        static DataSourceController controller =
            new DataSourceController(new MockDatasetRepository());

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(MockDatasetRepository.Context, controller.GetDatasets());
        }

        [TestMethod]
        public void TestMethod2()
        {
            var expected = MockDatasetRepository.Context[0];
            var actual = controller.GetDatasetById("1");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            try
            {
                controller.GetDatasetById("0");
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.NotFound);
                return;
            }

            Assert.Fail();
        }


    }
}
