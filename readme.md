# ExpresionBuiler readme

## A fast way to create Linq expressions

This project allows you to create a list of expressions, and compile the to a single AND / OR linq query. very usefall when working whit dynamic queries.

## Examples

First create a list of expressions, each expressions is comprised of a Func of an object, UserModel in the exmpales below, and bool.

<pre>
var andList = new List&lt;Expression&lt;Func&lt;UserModel,bool&gt;&gt;&gt;();
andList.Add((o) => o.Last != "smith" && o.Age > 5);
andList.Add((o) => o.Age > 5);
andList.Add((o) => o.isTemp);
andList.Add((o) => o.Rank == 1);

var orList = new List&lt;Expression&lt;Func&lt;UserModel,bool&gt;&gt;&gt;();
andList.Add((o) => o.Age !=12);
andList.Add((o) => o.Rank >2 1);
</pre>

### AND query
<pre>
var query=ExpresionTreeBuilder.CreateANDQuery&lt;UserModel&gt(andList).Compile();
var test = allUsers.Where(query);
</pre>

Or create a compiled query.

<pre>
var query=ExpresionTreeBuilder.CreateCompiledANDQuery&lt;UserModel&gt(andList);
var test = allUsers.Where(query);
</pre>

### OR query
<pre>
var query=ExpresionTreeBuilder.CreateORQuery&lt;UserModel&gt(OrList).Compile();
var test = allUsers.Where(query);
</pre>

Or create a compiled query.

<pre>
var query=ExpresionTreeBuilder.CreateCompiledOrQuery&lt;UserModel&gt(OrList);
var test = allUsers.Where(query);
</pre>

### Mixed AND and OR query
<pre>
var query=ExpresionTreeBuilder.CreateQuery&lt;UserModel&gt(andList,OrList).Compile();
var test = allUsers.Where(query);
</pre>

<pre>
var query=ExpresionTreeBuilder.CreateCompiledQuery&lt;UserModel&gt(andList,OrList);
var test = allUsers.Where(query);
</pre>

### Properties sorting

Each of the above methods can recive an optional bool param named sortProperties, deafult value is True.
When this param is set to True it will sort the order of the queries in the list to make sure the 'easiest' come first.

the order is:
1. bool
2. Integers (Int and long)
4. Floating point (Float, Double, Decimal)
3. string and Guid
4. datetime
5. other

## Order query

You can also create order by queries.
<pre>
var orderAscList = new List&lt;Expression&lt;Func&lt;UserModel,dynamic&gt;&gt;&gt;();
var orderDescList2 = new List&lt;Expression&lt;Func&lt;UserModel,dynamic&gt;&gt;&gt;();
orderAscList.Add((o) => o.Age);
orderDescList2.Add((o) => o.Rank);
</pre>
### Order by Asc
<pre>
allUsers = ExpresionTreeBuilder.CreateOrderASCQuery&lt;UserModel&gt;(allUsers, orderAscList);
</pre>
### Order by Desc
<pre>
allUsers = ExpresionTreeBuilder.CreateOrderDescQuery&lt;UserModel&gt;(allUsers, orderDescList2);
</pre>
### Order by Asc and Desc
<pre>
allUsers = ExpresionTreeBuilder.CreateOrderQuery&lt;UserModel&gt;(allUsers,orderAscList,orderDescList2,false);
</pre>
