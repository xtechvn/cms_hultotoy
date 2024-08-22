

var obj =
{
    "clientID": null,
    "createDateFrom": null,
    "createDateTo": null,
    "pageIndex" : 1,
    "pageSize" : 10
}

var _comment =
{
    LoadComment: function () {
        let url = '/Comment/ListComment';
        _ajax_caller.post(url, null, function (result) {
            $('#Comment-content').html(``);
            $('#Comment-content').append(result);
        });
    },

    GetComment: function ()
    {
        let url = '/Comment/GetAllComment';
        _ajax_caller.post(url, { request: obj }, function (result) {
            $('#table-head').html(``);
            $('#table-head').append(`
            <tr class="bg-main2" style="background-color:#F3F5F8">
            <th class="thead-fields">STT</th>
            <th class="thead-fields">KHÁCH HÀNG</th>
            <th class="thead-fields">NỘI DUNG CÂU HỎI - PHẢN HỒI </th>
            <th class="thead-fields">NGÀY TẠO</th>
            </tr>
            `);
            $('#table-comment-body').html(``);
            if (result != null && result.length > 0) {
                var STT = 1;
                result.forEach(item => {
                    $('#table-comment-body').append(`
                    <tr>
                    <td class="OptionRow trow-fields">${STT}</td>
                    <td class="OptionRow">
                        <div style="display: flex; flex-direction: column;">
                            <h5>${item.clientName}</h5>
                            <h7>${item.phone}</h7>
                            <h7>${item.email}</h7>
                        </div>
                    </td>
                    <td class="OptionRow" style="max-width:500px">
                        <div class="">${item.content}</div>
                    </td>
                    <td class="OptionRow">
                        <div class="trow-fields">${item.createdDate}</div>
                    </td>

                    </tr>
                    `);
                    STT += 1;
                })
            }
            else
            {
                $('#table-head').html(``);
                $('#table-head').append(`
                <div class="search-null center mb40">
                    <div class="mb24">
                        <img src="/images/graphics/icon-search.png" alt="">
                    </div>
                    <h2 class="title txt_24">Không tìm thấy kết quả</h2>
                    <div class="gray">Chúng tôi không tìm thấy thông tin mà bạn cần, vui lòng thử lại</div>
                </div>
                `);
            }
        });
    },
    getDateTimeRanges: function (type) {
        const now = moment();
        const startOfDay = now.clone().startOf('day');
        const endOfDay = now.clone().endOf('day');
        switch (type) {
            case 'today':
                return { start: startOfDay, end: endOfDay };
            case 'yesterday':
                return { start: startOfDay.clone().subtract(1, 'day'), end: endOfDay.clone().subtract(1, 'day') };
            case 'thisWeek':
                return { start: startOfDay.clone().startOf('week'), end: endOfDay.clone().endOf('week') };
            case 'lastWeek':
                return { start: startOfDay.clone().subtract(1, 'week').startOf('week'), end: startOfDay.clone().subtract(1, 'week').endOf('week') };
            case 'thisMonth':
                return { start: startOfDay.clone().startOf('month'), end: endOfDay.clone().endOf('month') };
            case 'lastMonth':
                return { start: startOfDay.clone().subtract(1, 'month').startOf('month'), end: startOfDay.clone().subtract(1, 'month').endOf('month') };
            case 'thisQuarter':
                return { start: startOfDay.clone().startOf('quarter'), end: endOfDay.clone().endOf('quarter') };
            case 'lastQuarter':
                return { start: startOfDay.clone().subtract(1, 'quarter').startOf('quarter'), end: startOfDay.clone().subtract(1, 'quarter').endOf('quarter') };
            default:
                return { start: moment('2024-01-01', 'YYYY-MM-DD'), end: endOfDay };
        }
    },

    OnchangeConditions: function ()
    {
        obj.pageSize = $("#selectPaggingOptions").find(':selected').val();
        obj.clientID = $("#clientInput").find(':selected').val();
        var DateSelected = this.getDateTimeRanges($("#DateInput").find(':selected').val());
        obj.createDateFrom = DateSelected.start.format('YYYY-MM-DD HH:mm:ss');
        obj.createDateTo = DateSelected.end.format('YYYY-MM-DD HH:mm:ss');
        console.log(obj);
        this.GetComment();
    },
}

$(document).ready(function () {
    $("#clientInput").select2({
        //theme: 'bootstrap4',
        placeholder: "Tìm kiếm theo tên,số điện thoại khách hàng",
        ajax: {
            url: "/Comment/Client",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    phoneOrName: params.term,
                }
                return query;
            },
            processResults: function (response) {
                $("#clientInput").empty();
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.clientname + " - " + item.phone,
                            id: item.id,
                        }
                    })
                };
            },
        }
    });
    var InputClientElement = $('.select2-selection__arrow')
    InputClientElement[0].classList.add('ClientInput_Arrow'); 
    _comment.GetComment();
    _comment.LoadComment();
});
