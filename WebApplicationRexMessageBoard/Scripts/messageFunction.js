$(function () {

    $.fn.deleteMessage = function (id, element, token) {

        if (!id) {
            return;
        }
        
        var reply = confirm('確認刪除留言?')
        if (!reply) {
            return;
        }

        $.ajax({
            url: 'Delete/'+id,
            type: 'POST',
            data: {
                __RequestVerificationToken: token
            },
            error: function (xhr) {
                alert('Ajax request 發生錯誤');
            },
            success: function (response) {
                // 補作錯誤處理

                if (element) {
                    element.remove();
                }
                
            }
        });
    };
});


