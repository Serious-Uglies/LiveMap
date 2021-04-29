(async function () {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl('/hub/livemap')
    .build();

  connection.on('SendLog', (request) => {
    $('#log').append(`<li>${request.message}</li>`);
  });

  await connection.start();
})();
