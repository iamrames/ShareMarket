import { ResultTypeOption } from './../enums/result-type-option.enum';
export interface DataResult<T> {
    data: T;
    message: string;
    resultType: ResultTypeOption;
}
