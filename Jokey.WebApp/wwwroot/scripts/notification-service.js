let connection;

export function startNotifications(url, dotNetRef) {
    if (connection && connection.state !== "Disconnected") {
        return;
    }

    connection = new signalR.HubConnectionBuilder()
                            .withUrl(url)
                            .withAutomaticReconnect([0, 2000, 5000])
                            .build();

    connection.on("ReceiveNotification", (notification) => {
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync("ReceiveNotification", notification);
        }
    });

    return connection.start();
}

export function stopNotifications() {
    if (connection) {
        const c = connection;
        connection = null;
        return c.stop();
    }
}