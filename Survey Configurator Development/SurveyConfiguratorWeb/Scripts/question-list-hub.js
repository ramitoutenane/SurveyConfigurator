
function GetQuestionList() {
    $.ajax({
        url: '/Survey/QuestionList',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            if (data != null) {
                Data = data;
                SortTable(SortOption);
            }
        },
        error: function () {
            ShowTableError();
        }
    });
}


function SetupConnection(hubProxy) {
    hubProxy.client.UpdateQuestionList = function () {
        GetQuestionList();
    }
}

//Start SignalR connection
$(document).ready(() => {
    let hubProxy = $.connection.questionListHub;
    SetupConnection(hubProxy);
    $.connection.hub.start();
})
