import { IAddress } from './address';

export interface IOrderToCreate {
    basketId: string;
    deliveryMethodId: number;
    shipToAddress: IAddress;
  }

  export interface IOrder {
    id: number;
    buyerEmail: string;
    orderDate: string;
    shipToAddress: IAddress;
    deliveryMethod: IDeliveryMethod;
    shippingPrice: number;
    orderItems: IOrderItem[];
    subTotal: number;
    total: number;
    status: number;
    paymentIntentId: string;
  }
  export interface IDeliveryMethod{
       shortName: string,
       deliveryTime: string,
       description: string,
       price: number
  }
  export interface IItemOrdered{
    productItemId: number;
    productName: string;
    pictureUrl: string;
  }
  export interface IOrderItem {
    itemOrdered: IItemOrdered;
    price: number;
    quantity: number;
    productItemId: number;
    productName: string;
    pictureUrl: string;
  }
  