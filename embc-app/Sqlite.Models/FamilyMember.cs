namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public sealed partial class FamilyMember : Evacuee
    {
        public bool SameLastNameAsEvacuee { get; set; }

        // related entities
        public string RelationshipToEvacueeCode { get; set; }

        public FamilyRelationshipType RelationshipToEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = Person.FAMILY_MEMBER;
        }
    }
}
