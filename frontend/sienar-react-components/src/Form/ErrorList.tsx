import Icon from '@/Icon';

interface ErrorListProps {
	errors: string[],
	errorIcon?: string
}

export default function ErrorList({errors, errorIcon = 'circle-exclamation'}: ErrorListProps) {
	return errors.length > 0 && (
		<ul className='form-error-list'>
			{errors.map(e => (
				<li
					className='form-error-list__error-wrapper'
					key={e}
				>
					<span className='form-error-list__error-icon-wrapper'>
						<Icon 
							className='form-error-list__error-icon'
							icon={errorIcon}
						/>
					</span>

					<span className='form-error-list__error-text'>
						{e}
					</span>
				</li>
			))}
		</ul>
	) || null;
}