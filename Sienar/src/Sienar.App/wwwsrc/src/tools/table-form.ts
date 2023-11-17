const tableForm = document.getElementById('table-search-form') as HTMLFormElement;
const searchTerm = document.getElementById('search-term') as HTMLInputElement;
const resetButton = document.getElementById('reset-search-term') as HTMLButtonElement|null;

resetButton?.addEventListener('click', (event: MouseEvent) => {
	event.preventDefault();

	searchTerm.value = '';
	tableForm.submit();
});

export {}