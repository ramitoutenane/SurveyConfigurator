function ShowDeleteDialog(pQuestionId) {
    $("#pId").val(pQuestionId);
    $("#DeleteConfirmation").modal();
}

$(document).ready(() => {
    GetQuestionList();
});