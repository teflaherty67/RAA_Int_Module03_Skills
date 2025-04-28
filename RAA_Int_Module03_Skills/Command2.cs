using Autodesk.Revit.DB.Architecture;

namespace RAA_Int_Module03_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Command2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document curDoc = uidoc.Document;

            // 01. select room
            Reference curRef = uiapp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Select a room");
            Room curRoom = curDoc.GetElement(curRef) as Room;

            // 02. create reference array & point list
            ReferenceArray refArray = new ReferenceArray();
            List<XYZ> pointList = new List<XYZ>();

            // 03. set boundary options
            SpatialElementBoundaryOptions sebOptions = new SpatialElementBoundaryOptions();
            sebOptions.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

            // 03a. get room boundaries
            List<BoundarySegment> boundarySegments = curRoom.GetBoundarySegments(sebOptions).First().ToList();




            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand2";
            string buttonTitle = "Button 2";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 2");

            return myButtonData.Data;
        }
    }

}
