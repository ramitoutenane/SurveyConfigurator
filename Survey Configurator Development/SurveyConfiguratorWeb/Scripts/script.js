function GetQuestionList() {
    $.ajax({
        url:'/Survey/QuestionList',
        type: 'GET',
        contentType: 'application/json',
        cache: false,
        success: function (data) {
            PopulateTable(data);
        },
        error: function () {
            ShowTableError();
        }
    });
}

function PopulateTable(pData) {
    if (pData == null)
        console.log("null")
    else
        console.log(pData);
};
function ShowTableError() {
    console.log("Error");
}

window.setInterval(function () {
    GetQuestionList();
}, 3000);