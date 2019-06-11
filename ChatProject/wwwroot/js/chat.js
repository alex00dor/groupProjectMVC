if(currentRoomId != null){
    ScrolldownMessagesBox();
    initSendButton();
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start();

connection.on("chat", function (userName, message, date, roomId, roomName) {
    console.log(userName, message, date, roomId, roomName);
    if(currentRoomId != null && currentRoomId === roomId){
        showMessageInRoom(userName, message, date, currentUserName === userName);
        var scroll=$('#messages');
        scroll.animate({scrollTop: scroll.prop("scrollHeight")});
    }else{
        showNotification(userName, message, date, roomName);
    }
});

function initSendButton() {
    $(document).ready(function(){
        $('#sendButton').attr('disabled',true);
        $('#messageText').keyup(function(){
            if($(this).val().length !== 0)
                $('#sendButton').attr('disabled', false);
            else
                $('#sendButton').attr('disabled',true);
        })
    });
    
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var message = document.getElementById("messageText").value;
        document.getElementById("messageText").value = '';
        $('#sendButton').attr('disabled',true);
        connection.invoke("SendMessage", currentRoomId, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}

function ScrolldownMessagesBox() {
    try {
        document.getElementById("messages").scrollTo(0, document.getElementById("messages").scrollHeight);
    } catch (e) {
        var scroll=$('#messages');
        scroll.animate({scrollTop: scroll.prop("scrollHeight")});
    }
}

function showMessageInRoom(userName, message, date, sender) {
    var appendHtml = '<div class="row"><div class="col-12 px-4 pt-2">' +
        '<div class="'+(sender? 'float-right' : 'float-left')+'">' +
        '<div class="message '+(sender? 'message-sender':'')+'">' +
        '<span class="badge badge-pill badge-light float-left">'+userName+':</span>' +
        '<br>'+message+'</div><div class="message-date">'+date+'</div>' +
        '</div>' +
        '</div></div>';
    $('#messages').append(appendHtml);
}

var toastCount = 0;
function showNotification(userName, message, date, roomName) {
    var appendHtml = '<div id="toast'+toastCount+'" aria-atomic="true" data-delay="10000" class="toast ml-auto" role="alert" style="min-width: 300px; z-index:101;">' +
        '            <div class="toast-header">' +
        '                <strong class="mr-auto">'+roomName+'</strong>' +
        '                  <small>'+date+'</small>' +
        '                  <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">' +
        '                    <span aria-hidden="true">&times;</span>' +
        '                  </button>' +
        '                </div>' +
        '                <div class="toast-body">'+userName+': '+message+'</div>' +
        '        </div>';
    $('#notifications').append(appendHtml);
    $('#toast'+toastCount).toast('show');
    toastCount++;
}