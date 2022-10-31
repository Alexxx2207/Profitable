import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const calculateStocksTrade = (
    buyPrice,
    sellPrice,
    numberOfShares,
    buyCommission,
    sellCommission
) => {
    return request
        .post(`${WEB_API_BASE_URL}/positions/calculate/stocks`, {
            buyPrice,
            sellPrice,
            numberOfShares,
            buyCommission,
            sellCommission,
        })
        .then((response) => response.json());
};
