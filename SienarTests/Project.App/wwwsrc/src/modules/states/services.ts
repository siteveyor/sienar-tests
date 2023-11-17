import {CrudService} from '@/utils/services';
import {EntityBase} from '@/utils/types';

export interface StateDto extends EntityBase {
	name: string
	abbreviation: string
}

export const stateService = new CrudService<StateDto>('/sienar/states', 'state');