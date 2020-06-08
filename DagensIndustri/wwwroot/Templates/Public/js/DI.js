/*
 * Javascript functions
 */
 
//-- Dynamically set validation group on a validation summary control.
function SetValidationGroup(validationGroupName) {
    //-- Check for null validation group name
    if (validationGroupName == null) {
        return;
    }

    if (Page_ValidationSummaries.length > 0) {
        //-- Assign validation group name to validationSummary control
        Page_ValidationSummaries[0].validationGroup = validationGroupName;
    }
}