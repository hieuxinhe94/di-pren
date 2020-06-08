using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.DataAccess.Package;
using Pren.Web.Business.DataAccess.Package.Entities;
using Pren.Web.Business.Package;
using Pren.Web.UnitTests.Fakes;

namespace Pren.Web.UnitTests.Business.Package
{
    [TestClass]
    public class PackageRelationManagerUnitTests
    {

        private IPackageRelationManager GetPackageRelationManager(IPackageRelationItemDataHandler packageRelationItemDataHandler = null)
        {
            return new PackageRelationManager(packageRelationItemDataHandler ?? FakeService.GetFakePackageRelationItemDataHandler().Object);
        }

        private IPackageRelationManager GetPackageRelationManagerWithItemHandlerMock(IEnumerable<PackageRelationItemEntity> itemEntities)
        {
            var itemHandlerMock = new Mock<IPackageRelationItemDataHandler>();

            itemHandlerMock.Setup(t => t.GetPackageRelationItems(It.IsAny<int>(), It.IsAny<string>())).Returns(itemEntities);

            return GetPackageRelationManager(itemHandlerMock.Object);
        }


        [TestMethod]
        public void GetParameters_WithOnlyWildcard_ReturnsParametersWithWildcard()
        {
            var itemEntities = new List<PackageRelationItemEntity>
            {
                new PackageRelationItemEntity
                {
                    Name = "IPAD_01",
                    WildcardBefore = true,
                    WildcardAfter = true,
                },
                new PackageRelationItemEntity
                {
                    Name = "IPAD_02",
                    WildcardBefore = true,
                    WildcardAfter = true,
                },
            };

            var packageRelationManager = GetPackageRelationManagerWithItemHandlerMock(itemEntities);

            Assert.AreEqual("*-IPAD_01-*,*-IPAD_02-*", packageRelationManager.GetParameters(It.IsAny<PackageRelationTypeEnum>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void GetParameters_WithWildcardBeforeAndConditionsAfter_ReturnsParametersWithWildcardBeforeAndConditionAfter()
        {
            var itemEntities = new List<PackageRelationItemEntity>
            {
                new PackageRelationItemEntity
                {
                    Name = "IPAD_01",
                    WildcardBefore = true,
                    WildcardAfter = false,
                    ConditionAfter = "A"
                },
                new PackageRelationItemEntity
                {
                    Name = "IPAD_02",
                    WildcardBefore = true,
                    WildcardAfter = false,
                    ConditionAfter = "P"
                },
            };

            var packageRelationManager = GetPackageRelationManagerWithItemHandlerMock(itemEntities);

            Assert.AreEqual("*-IPAD_01-A,*-IPAD_02-P", packageRelationManager.GetParameters(It.IsAny<PackageRelationTypeEnum>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void GetParameters_WithConditionBeforeAndWildcardAfter_ReturnsParametersWithConditionBeforeAndWildcardAfter()
        {
            var itemEntities = new List<PackageRelationItemEntity>
            {
                new PackageRelationItemEntity
                {
                    Name = "IPAD_01",
                    WildcardBefore = false,
                    ConditionBefore = "PAPER",
                    WildcardAfter = true,
                },
                new PackageRelationItemEntity
                {
                    Name = "IPAD_02",
                    WildcardBefore = false,
                    ConditionBefore = "DIGITAL",
                    WildcardAfter = true,
                },
            };

            var packageRelationManager = GetPackageRelationManagerWithItemHandlerMock(itemEntities);

            Assert.AreEqual("PAPER-IPAD_01-*,DIGITAL-IPAD_02-*", packageRelationManager.GetParameters(It.IsAny<PackageRelationTypeEnum>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void GetParameters_WithConditionBeforeWildcardAfterAndWildcardBeforeConditionAfter_ReturnsParametersWithConditionBeforeWildcardAfterAndWildcardBeforeConditionAfter()
        {
            var itemEntities = new List<PackageRelationItemEntity>
            {
                new PackageRelationItemEntity
                {
                    Name = "IPAD_01",
                    WildcardBefore = false,
                    ConditionBefore = "PAPER",
                    WildcardAfter = true,
                },
                new PackageRelationItemEntity
                {
                    Name = "IPAD_02",
                    WildcardBefore = true,                    
                    WildcardAfter = false,
                    ConditionAfter = "A",
                },
            };

            var packageRelationManager = GetPackageRelationManagerWithItemHandlerMock(itemEntities);

            Assert.AreEqual("PAPER-IPAD_01-*,*-IPAD_02-A", packageRelationManager.GetParameters(It.IsAny<PackageRelationTypeEnum>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void GetParameters_WithConditionBeforeAndConditionAfter_ReturnsParametersWithConditionBeforeAndConditionAfter()
        {
            var itemEntities = new List<PackageRelationItemEntity>
            {
                new PackageRelationItemEntity
                {
                    Name = "IPAD_01",
                    WildcardBefore = false,
                    ConditionBefore = "PAPER",
                    WildcardAfter = false,
                    ConditionAfter = "P"
                },
                new PackageRelationItemEntity
                {
                    Name = "IPAD_02",
                    WildcardBefore = false,                    
                    ConditionBefore = "DIGITAL",
                    WildcardAfter = false,
                    ConditionAfter = "A",
                },
            };

            var packageRelationManager = GetPackageRelationManagerWithItemHandlerMock(itemEntities);

            Assert.AreEqual("PAPER-IPAD_01-P,DIGITAL-IPAD_02-A", packageRelationManager.GetParameters(It.IsAny<PackageRelationTypeEnum>(), It.IsAny<string>()));
        }
    }
}
