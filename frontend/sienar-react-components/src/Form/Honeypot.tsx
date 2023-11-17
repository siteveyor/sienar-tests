interface HoneypotProps {
	value: string,
	setValue: (newValue: string) => void
}

export default function Honeypot({value, setValue}: HoneypotProps) {
	return (
		<input
			className='input--secret-key-field'
			value={value}
			onChange={e => setValue(e.target.value)}
			tabIndex={-1}
		/>
	);
}