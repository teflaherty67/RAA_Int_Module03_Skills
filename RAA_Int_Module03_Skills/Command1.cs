namespace RAA_Int_Module03_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document curDoc = uidoc.Document;

            // 01. get model lines
            FilteredElementCollector colModelLines = new FilteredElementCollector(curDoc, curDoc.ActiveView.Id)
                .OfClass(typeof(CurveElement));

            // 02. create reference array & point list
            ReferenceArray newArray = new ReferenceArray();
            List<XYZ> pointsList = new List<XYZ>();

            // 03. loop through the lines
            foreach(ModelLine curLine in colModelLines)
            {
                // 03a. get the midpoint of the line
                Curve curCurve = curLine.GeometryCurve;
                XYZ midPoint = curCurve.Evaluate(0.5, true);

                // 03b. add lines to reference array
                newArray.Append(new Reference(curLine));

                // 03c. add midpoint to point list
                pointsList.Add(midPoint);

            }

            // 04. oder list left to right
            List<XYZ> sortedList = pointsList.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            // 04a. get location of first & last line in sorted list
            XYZ point1 = sortedList.First();
            XYZ point2 = sortedList.Last();

            // 05. create line for dimension string location
            Line dimLine = Line.CreateBound(point1, point2);

            // 06. create tranmsaction & add dimension
            using (Transaction t = new Transaction (curDoc))
            {
                t.Start("Create dimension");
                Dimension newDim = curDoc.Create.NewDimension(curDoc.ActiveView, dimLine, newArray);

                t.Commit();
            }

            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData.Data;
        }
    }

}
