﻿//---------------------------------------------------------------------
// <copyright file="ODataSelectPathTests.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.OData.UriParser;
using Xunit;
using ODataErrorStrings = Microsoft.OData.Strings;

namespace Microsoft.OData.Tests.UriParser.SemanticAst
{
    public class ODataSelectPathTests
    {
        [Fact]
        public void SelectPathShouldNotAllowCountSegment()
        {
            Action buildWithCountSegment = () => new ODataSelectPath(CountSegment.Instance);
            buildWithCountSegment.Throws<ODataException>(ODataErrorStrings.ODataSelectPath_InvalidSelectPathSegmentType("CountSegment"));
        }

        [Fact]
        public void SelectPathShouldNotAllowValueSegment()
        {
            Action buildWithCountSegment = () => new ODataSelectPath(new ValueSegment(ModelBuildingHelpers.BuildValidEntityType()));
            buildWithCountSegment.Throws<ODataException>(ODataErrorStrings.ODataSelectPath_InvalidSelectPathSegmentType("ValueSegment"));
        }

        [Fact]
        public void SelectPathShouldAllowMultipleDifferentSegments()
        {
            List<ODataPathSegment> typeSegments = new List<ODataPathSegment>() { new TypeSegment(HardCodedTestModel.GetPersonType(), null), new NavigationPropertySegment(HardCodedTestModel.GetPersonMyDogNavProp(), null) };
            new ODataSelectPath(typeSegments).ContainExactly(typeSegments.ToArray());
        }

        [Fact]
        public void SelectPathShouldnotEndInTypeSegment()
        {
            List<ODataPathSegment> typeSegments = new List<ODataPathSegment>() { new TypeSegment(HardCodedTestModel.GetPersonType(), null) };
            Action createWithTypeAsLastSegment = () => new ODataSelectPath(typeSegments);
            createWithTypeAsLastSegment.Throws<ODataException>(ODataErrorStrings.ODataSelectPath_CannotOnlyHaveTypeSegment);
        }

        [Fact]
        public void NavPropCanOnlyBeLastSegment()
        {
            List<ODataPathSegment> typeSegments = new List<ODataPathSegment>() { new TypeSegment(HardCodedTestModel.GetPersonType(), null), new NavigationPropertySegment(HardCodedTestModel.GetPersonMyDogNavProp(), null), new PropertySegment(HardCodedTestModel.GetDogColorProp()) };
            Action createWithInteriorNavProp = () => new ODataSelectPath(typeSegments);
            createWithInteriorNavProp.Throws<ODataException>(ODataErrorStrings.ODataSelectPath_NavPropSegmentCanOnlyBeLastSegment);
        }

        [Fact]
        public void OperationCanOnlyBeLastSegment()
        {
            List<ODataPathSegment> typeSegments = new List<ODataPathSegment>() { new TypeSegment(HardCodedTestModel.GetPersonType(), null), new OperationSegment(HardCodedTestModel.GetFunctionForGetCoolPeople(), HardCodedTestModel.GetPeopleSet()), new PropertySegment(HardCodedTestModel.GetDogColorProp()) };
            Action createWithInteriorNavProp = () => new ODataSelectPath(typeSegments);
            createWithInteriorNavProp.Throws<ODataException>(ODataErrorStrings.ODataSelectPath_OperationSegmentCanOnlyBeLastSegment);
        }

        [Fact]
        public void OperationImportCanOnlyBeLastSegment()
        {
            List<ODataPathSegment> typeSegments = new List<ODataPathSegment>() { new OperationImportSegment(HardCodedTestModel.GetFunctionImportForGetCoolestPerson(), HardCodedTestModel.GetPeopleSet())};
            Action createWithInteriorNavProp = () => new ODataSelectPath(typeSegments);
            createWithInteriorNavProp.Throws<ODataException>(ODataErrorStrings.ODataSelectPath_InvalidSelectPathSegmentType(typeof(OperationImportSegment).Name));
        }
    }
}