To access protected APIs, you need to generate an authentication token. Use the following endpoint to generate a token:
api/Token/generate

The Get ALL list APIs can be filtered based on the parameters or sorted based on the paramter type asc\desc

example(filter):
https://localhost:7051/api/Auction/properties?name=Propery2
Some examples(sort):
https://localhost:7051/api/Auction/properties?sortBy=Name&sortDirection=desc
https://localhost:7051/api/Auction/Bids?sortBy=Name&sortDirection=desc OR https://localhost:7051/api/Auction/Bids?sortBy=BidTime&sortDirection=desc





