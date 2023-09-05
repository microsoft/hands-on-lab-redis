export default interface Product {
    id: string;
    title: string;
    description: string;
    image?: string;
    quantity?: number;
    price?: number;
    _ts: number;
}
