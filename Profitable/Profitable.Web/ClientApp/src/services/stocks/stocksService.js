export const calculateStockTrade = (
    numberOfShares,
    sellPrice,
    buyPrice,
    sellCommission,
    buyCommission
) => {
    return (
        numberOfShares * sellPrice - sellCommission - (numberOfShares * buyPrice + buyCommission)
    );
};
