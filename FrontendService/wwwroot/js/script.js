document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInput');
    const fileInput = document.getElementById('fileInput');
    const uploadButton = document.getElementById('uploadButton');
    const nameHeader = document.getElementById('nameHeader');
    const dobHeader = document.getElementById('dobHeader');
    const marriedHeader = document.getElementById('marriedHeader');
    const phoneHeader = document.getElementById('phoneHeader');
    const salaryHeader = document.getElementById('salaryHeader');
    let isAsc = { name: true, dob: true, married: true, phone: true, salary: true };

    // Завантаження контактів з DataService
    fetch('http://localhost:5001/api/contacts')
        .then(response => response.json())
        .then(data => {
            const tbody = document.getElementById('contactsBody');
            tbody.innerHTML = ''; // Очищуємо старі дані
            data.forEach(contact => {
                const row = document.createElement('tr');
                row.setAttribute('data-id', contact.id);

                row.innerHTML = `
                    <td>${contact.name}</td>
                    <td>${formatDate(contact.dateOfBirth)}</td> <!-- Дата у форматі yyyy-mm-dd -->
                    <td>${contact.married ? '✅' : '❌'}</td>
                    <td>${contact.phone}</td>
                    <td>${contact.salary}</td>
                    <td class="text-center">
                        <button class="btn btn-sm btn-primary edit-btn">Edit</button>
                        <button class="btn btn-sm btn-danger delete-btn">Delete</button>
                    </td>
                `;
                tbody.appendChild(row);
            });
        });
    function formatDate(dateString) {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0'); // додаємо 1, оскільки місяці починаються з 0
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`; // Повертаємо формат yyyy-mm-dd
    }

    // Функція завантаження CSV-файлу
    uploadButton.addEventListener('click', () => {
        const file = fileInput.files[0];
        if (!file) {
            alert('Please select a file.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        fetch('http://localhost:5002/api/fileupload/upload', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    alert('File uploaded successfully.');
                    location.reload()
                } else {
                    alert('Failed to upload file.');
                }
            })
            .catch(error => {
                console.error('Error uploading file:', error);
            });
    });

    // Сортування колонок таблиці
    nameHeader.addEventListener('click', function () {
        clearSortIndicators();
        sortTable(1, 'text', isAsc.name);
        isAsc.name = !isAsc.name;
        updateSortDirection(nameHeader, isAsc.name);
    });

    dobHeader.addEventListener('click', function () {
        clearSortIndicators();
        sortTable(2, 'date', isAsc.dob);
        isAsc.dob = !isAsc.dob;
        updateSortDirection(dobHeader, isAsc.dob);
    });

    marriedHeader.addEventListener('click', function () {
        clearSortIndicators();
        sortTable(3, 'text', isAsc.married);
        isAsc.married = !isAsc.married;
        updateSortDirection(marriedHeader, isAsc.married);
    });

    phoneHeader.addEventListener('click', function () {
        clearSortIndicators();
        sortTable(4, 'text', isAsc.phone);
        isAsc.phone = !isAsc.phone;
        updateSortDirection(phoneHeader, isAsc.phone);
    });

    salaryHeader.addEventListener('click', function () {
        clearSortIndicators();
        sortTable(5, 'number', isAsc.salary);
        isAsc.salary = !isAsc.salary;
        updateSortDirection(salaryHeader, isAsc.salary);
    });

    function sortTable(columnIndex, type, isAsc) {
        const rows = Array.from(document.querySelectorAll('#contactsBody tr'));
        rows.sort((a, b) => {
            let valA = a.querySelector(`td:nth-child(${columnIndex})`).textContent.trim();
            let valB = b.querySelector(`td:nth-child(${columnIndex})`).textContent.trim();

            if (type === 'date') {
                valA = new Date(valA);
                valB = new Date(valB);
            } else if (type === 'number') {
                valA = parseFloat(valA);
                valB = parseFloat(valB);
            }

            if (valA < valB) return isAsc ? -1 : 1;
            if (valA > valB) return isAsc ? 1 : -1;
            return 0;
        });

        const tbody = document.getElementById('contactsBody');
        rows.forEach(row => tbody.appendChild(row));
    }

    function clearSortIndicators() {
        document.querySelectorAll('.sortable').forEach(header => {
            header.innerHTML = header.textContent.split(' ')[0];
        });
    }

    function updateSortDirection(header, isAsc) {
        const arrow = isAsc ? ' ▲' : ' ▼';
        header.innerHTML = header.textContent.split(' ')[0] + arrow;
    }

    // Пошук по імені
    searchInput.addEventListener('input', function () {
        const filter = searchInput.value.toLowerCase();
        const rows = document.querySelectorAll('#contactsBody tr');

        rows.forEach(row => {
            const name = row.querySelector('td:first-child').textContent.toLowerCase();
            row.style.display = name.includes(filter) ? '' : 'none';
        });
    });

    // Видалення та редагування контактів
    document.querySelector('table').addEventListener('click', function (event) {
        if (event.target.classList.contains('edit-btn')) {
            handleEdit(event.target);
        } else if (event.target.classList.contains('save-btn')) {
            handleSave(event.target);
        } else if (event.target.classList.contains('delete-btn')) {
            handleDelete(event.target);
        }
    });

    function handleEdit(button) {
        event.preventDefault();
        var row = button.closest('tr');
        row.querySelectorAll('td').forEach(function (cell, index) {
            if (index < 5) {
                var input = document.createElement('input');

                switch (index) {
                    case 1: // Date of Birth
                        input.type = 'date'; // Поле дати, використовує стандартний формат yyyy-mm-dd
                        input.value = cell.textContent.trim();
                        break;
                    case 2: // Married
                        input.type = 'checkbox';
                        input.checked = cell.textContent.trim() === '✅';
                        break;
                    case 3: // Phone
                        input.type = 'tel';
                        input.value = cell.textContent.trim();
                        break;
                    case 4: // Salary
                        input.type = 'number';
                        input.step = '1.00';
                        input.value = parseFloat(cell.textContent.trim());
                        break;
                    default:
                        input.type = 'text';
                        input.value = cell.textContent.trim();
                }

                input.classList.add('table-input');
                cell.innerHTML = '';
                cell.appendChild(input);
            }
        });

        button.textContent = 'Save';
        button.classList.replace('edit-btn', 'save-btn');
    }

    function handleSave(button) {
        event.preventDefault();

        var row = button.closest('tr');
        var contact = {
            Id: parseInt(row.dataset.id),
            Name: row.querySelector('td:nth-child(1) input').value,
            DateOfBirth: row.querySelector('td:nth-child(2) input').value, // Дата у форматі yyyy-mm-dd
            Married: row.querySelector('td:nth-child(3) input').checked,
            Phone: row.querySelector('td:nth-child(4) input').value,
            Salary: parseFloat(row.querySelector('td:nth-child(5) input').value)
        };

        fetch('http://localhost:5001/api/contacts/' + contact.Id, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(contact)
        }).then(response => {
            if (response.ok) {
                console.log('Contact saved successfully');
                updateRow(row, contact);

                button.textContent = 'Edit';
                button.classList.replace('save-btn', 'edit-btn');
            } else {
                console.error('Failed to save contact.');
            }
        }).catch(error => console.error('Error:', error));
    }

    function handleDelete(button) {
        event.preventDefault();

        if (confirm("Are you sure you want to delete this contact?")) {
            var row = button.closest('tr');
            var id = row.dataset.id;

            fetch('http://localhost:5001/api/contacts/' + id, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    row.remove();
                    alert("Contact deleted successfully.");
                } else {
                    console.error('Failed to delete contact.');
                }
            }).catch(error => console.error('Error:', error));
        }
    }

    function updateRow(row, contact) {
        row.querySelectorAll('td').forEach(function (cell, index) {
            if (index < 5) {
                const values = [
                    contact.Name,
                    contact.DateOfBirth, // Дата у форматі yyyy-mm-dd
                    contact.Married ? '✅' : '❌',
                    contact.Phone,
                    contact.Salary.toFixed(2)
                ];
                cell.textContent = values[index];
            }
        });
    }
});
