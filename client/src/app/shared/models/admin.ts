import { IProduct } from './product';

export interface IAdminActionHistory {
       date: Date,
       adminEmail: string,
       operation: number,
       adminAction: string,
       productId?: number

}   
export interface IOrdersForDayWeekMounth{
     ordersToday?: number,
     ordersWeek?: number,
     ordersMounth?: number
}  
