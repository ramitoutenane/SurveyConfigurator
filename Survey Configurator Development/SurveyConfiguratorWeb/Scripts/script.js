let Data = {};
let SortedData = {};


function GetQuestionList(pHash) {
    $("#loader").removeClass("hidden")
    $("#TableError").addClass("hidden");
    $("#QuestionTable").addClass("hidden");
    $.ajax({
        url: '/Survey/QuestionList?Hash=' + pHash,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            if (data != null) {
                $("#loader").removeClass("hidden")
                $("#QuestionTable").addClass("hidden");
                Data = data;
                PopulateTable(data);
            }

        },
        error: function () {
            ShowTableError();
        }
    });
}

function PopulateTable(pData) {
    let tTableBody = $("#QuestionTableBody")[0];
    tTableBody.innerHTML = '';
    for (let i = 0; i < pData.length; i++) {
        let tRow = `
                    <tr>
                        <td>${pData[i].Text}</td>
                        <td>${pData[i].Order}</td>
                        <td>${pData[i].TypeString}</td>
                        <td>
                            <a href="/Survey/Edit/${pData[i].Id}" class="edit"  data-toggle="tooltip" > <i class="material-icons">&#xE254;</i></a >
                            <a href="#" onclick= ShowDeleteDialog(${pData[i].Id}); class="delete" data-toggle="tooltip"><i class="material-icons">&#xE872;</i></a>
                        </td>
                    </tr>
                    `
        tTableBody.innerHTML += tRow;
    }
    $("#loader").addClass("hidden")
    $("#TableError").addClass("hidden");
    $("#QuestionTable").removeClass("hidden");


};

function ShowTableError() {
    $("#TableError").removeClass("hidden");
    $("#loader").addClass("hidden")
    $("#QuestionTable").addClass("hidden");
}

function StartAutoRefresh(pInterval) {
    if (pInterval == null || isNaN(pInterval))
        pInterval = 20000;
    window.setInterval(function () {
        let tHash = md5((JSON.stringify(Data))).toUpperCase();
        GetQuestionList(tHash);
    }, pInterval);
}

$('th').on('click', function () {
    console.log("clicked")
    let tColumn = $(this).data('name')
    let tOrder = $(this).data('order')
    let tLabel = $(this).html()
    tLabel = tLabel.substring(0, tLabel.length - 1)

    if (tOrder == 'desc') {
        $(this).data('order', "asc")
        SortedData = Data.sort((a, b) => a[tColumn] > b[tColumn] ? 1 : -1)
        tLabel += '&#9660'

    } else {
        $(this).data('order', "desc")
        SortedData = Data.sort((a, b) => a[tColumn] < b[tColumn] ? 1 : -1)
        tLabel += '&#9650'

    }
    $(this).html(tLabel)
    PopulateTable(SortedData)
})