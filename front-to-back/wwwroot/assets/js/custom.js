$(function () {
    var skipRow = 1
    $(document).on("click",'#loadMore', function(){
        $.ajax({
            method: "GET",
            url: "/home/loadmore",
            data: {
                skipRow:skipRow
            },
            success: function (result) {
                $('#recentWorkComponents').append(result);
                skipRow++;
            }
            })
    })
})



$(function () {
    var skipRow = 1
    $(document).on('click', '#loadMorePricing', function () {

        $.ajax({
            method: "GET",
            url: "/pricing/loadmore",
            data: {
                skipRow: skipRow
            },
            success: function (result) {
                $('#ourpricing').append(result);
                skipRow++;
            }
        })
    })
})