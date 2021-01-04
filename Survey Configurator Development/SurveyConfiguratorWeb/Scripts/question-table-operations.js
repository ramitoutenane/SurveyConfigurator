let Data = {};
let SortedData = {};
let SortOption = null;
let AutoRefresh = null;

function PopulateTable(pData) {
    if (pData == null)
        return;
    $("#loader").removeClass("hidden")
    $("#TableError").addClass("hidden");
    $("#QuestionTable").addClass("hidden");
    let tTableBody = $("#QuestionTableBody")[0];
    tTableBody.innerHTML = '';
    for (let i = 0; i < pData.length; i++) {
        let tRow = `
                    <tr>
                        <td>${pData[i].Text}</td>
                        <td>${pData[i].Order}</td>
                        <td>${pData[i].TypeString}</td>
                        <td>
                            <a href="/Survey/Edit/${pData[i].Id}" class="edit" data-toggle="tooltip" > <i class="material-icons">&#xE254;</i></a >
                            <a href="#" onclick= ShowDeleteDialog(${pData[i].Id}); class="delete" data-toggle="tooltip"><i class="material-icons">&#xE872;</i></a>
                        </td>
                    </tr>
                    `
        tTableBody.innerHTML += tRow;
    }
    $("#loader").addClass("hidden")
    $("#QuestionTable").removeClass("hidden");
};

function ShowTableError() {
    $("#TableError").removeClass("hidden");
    $("#loader").addClass("hidden")
    $("#QuestionTable").addClass("hidden");
    if(AutoRefresh != null)
        clearInterval(AutoRefresh);
}

function SortTable(pSortOption) {
    if (pSortOption == null || pSortOption.value == "default") {
        SortOption = pSortOption;
        PopulateTable(Data);
    }
    else {
        $("#QuestionTable").addClass("hidden");
        $("#loader").removeClass("hidden")
        SortOption = pSortOption;
        tSortMethod = SortOption.value
        tOrder = $(SortOption).children("option:selected").data('order')
        SortedData = Data;
        if (tOrder == 'asc') {
            SortedData.sort((a, b) => a[tSortMethod] > b[tSortMethod] ? 1 : -1)
        } else {
            SortedData.sort((a, b) => a[tSortMethod] < b[tSortMethod] ? 1 : -1)
        }
        PopulateTable(SortedData)
    }
}
