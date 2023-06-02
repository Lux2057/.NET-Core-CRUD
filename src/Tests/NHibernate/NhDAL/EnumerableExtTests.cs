namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class EnumerableExtTests
{
    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[]
                     {
                             new[]
                             {
                                     new TestEntity { Id = 1 },
                                     new TestEntity { Id = 2 },
                                     new TestEntity { Id = 3 }
                             },

                             new[] { 1, 2, 3 }
                     };

        yield return new object[]
                     {
                             new[]
                             {
                                     new TestEntity { Id = 1 },
                                     new TestEntity { Id = 2 },
                                     new TestEntity { Id = 3 },
                                     new TestEntity { Id = 3 },
                             },

                             new[] { 1, 2, 3 }
                     };

        yield return new object[]
                     {
                             Array.Empty<TestEntity>(),
                             Array.Empty<int>()
                     };

        yield return new object[]
                     {
                             (TestEntity[])null,
                             Array.Empty<int>()
                     };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public void Should_return_expected_collection(IEnumerable<TestEntity> entities, int[] expected)
    {
        Assert.Equal(expected, entities.GetIds<TestEntity, int>());
    }
}