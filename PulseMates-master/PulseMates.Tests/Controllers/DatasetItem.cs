namespace PulseMates.Tests.Controllers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Models;
    using Models.Storage;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class DatasetItemTest
    {
        static readonly IDataSourceRepository _repository =
            new PulseMates.Infrastructure.Mongo.DataSourceRepository();

        [TestMethod]
        public void CreateNewDataSource()
        {
            var ds = new DataSource
            {
                Name = "Test",
                Description = "Hello, World!",
                Parameters = new List<IDataSourceParameter> 
                {
                    DataSource.Create<StringDataSourceParameter>("Title", "Hello, Definition!"),
                    DataSource.Create<ImageDataSourceParameter>("Image"),
                    DataSource.Create<LocationDataSourceParameter>("Location", "The address of where the event took place")
                }
            };

            ds = _repository.Create(ds);

            Assert.IsNotNull(ds.Id);
        }

        [TestMethod]
        public void UpdateDataSource()
        {
            var ds = _repository.FindAll().FirstOrDefault();
            var paramList = ds.Parameters.ToList();

            

            paramList.Add(DataSource.Create<NumberDataSourceParameter>("Value"));
            paramList[0].Name = "Title2";

            ds.Parameters = paramList;

            ds = _repository.Update(ds);
        }

        [TestMethod]
        public void FindSourceItems()
        {
            var ds = _repository.FindAll().FirstOrDefault();
            var items = _repository.FindAllItems(ds.Id).ToArray();


        }

        [TestMethod]
        public void CreateNewDataSouceItem()
        {
            var ds = _repository.FindAll().FirstOrDefault();
            var items = _repository.FindAllItems(ds.Id).ToArray();

            var item = ds.CreateValue();
            item["Title"].Value = "Test2";
            item["Image"].Value = new File { Filename = "test.png" };

            //item.Title = "Test2";
            //item.Title = "Test2";
            //item.Image = new File { Filename = "test.png" };

            item = _repository.Create(ds.Id, (DataSourceValue)item);

            //item.Title = "Update Test";
            //item.Image = new File { Filename = "test update.png" };

            item = _repository.Update(ds.Id, (DataSourceValue)item);


            

            Assert.IsNotNull(item.Id);
        }

        [TestMethod]
        public void DeleteDatasourceItem()
        {
            var ds = _repository.FindAll().FirstOrDefault();
            var item = _repository.FindItem(ds.Id, "50f7a57aaca77819ac017079");

            _repository.Delete(ds.Id, item.Id);
        }
    }
}
