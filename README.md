# Parcel Delivery Rates

The Big Courier Company delivers packages across the country, using the rates described below.
Write some code that will price a delivery order of up to three items from one address to one or more recipient addresses.

### Charges
There are three package types, and two delivery types. The rates are:


------------------------------------------------------
| Delivery | Documents | Small Parcel | Large Parcel |
| Same Day | €4,00     | €7,00        | €9,00        |
| Two Days | €1,00     | €2,50        | €3,00        |
------------------------------------------------------


### More Rules
There are some extra complications about how items are priced.
These are not listed in priority order, so you can choose which ones to implement first.
- An order with 3 Same Day items gets a 5% discount
- Same Day is not available for Vorarlberg and Tirol
- Premium clients get a 7.5% discount, except on Large Parcels
- The pickup address must be different from the delivery addresses
- An order with 3 items to the same address gets a 2% discount, but not in addition to the discount for 3 Same Day items
- Deliveries between Wien and Bregenz have a surcharge of 25%
- Delivery rates can change twice a year.
