function buildRoute() {
    const from = document.getElementById("from").value;
    const to = document.getElementById("to").value;

    if (from === to) {
        document.getElementById("result").innerText =
            "Ви вже в цьому кабінеті 🙂";
        return;
    }

    document.getElementById("result").innerHTML = `
    <b>Маршрут:</b><br>
    Вийдіть з кабінету ${from}<br>
    Перейдіть коридором<br>
    Зайдіть у кабінет ${to}
  `;
}
let rooms = [];

fetch("data/rooms.json")
    .then(response => response.json())
    .then(data => {
        rooms = data;
        const select = document.getElementById("roomSelect");

        rooms.forEach(room => {
            const option = document.createElement("option");
            option.value = room.number;
            option.textContent = room.number;
            select.appendChild(option);
        });
    });

function showRoom() {
    const selected = document.getElementById("roomSelect").value;
    const room = rooms.find(r => r.number === selected);

    if (!room) return;

    document.getElementById("info").innerText =
        `Кабінет ${room.number}, ${room.floor} поверх`;

    document.getElementById("mapImage").src =
        `assets/${room.map}`;
}
