<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="D3FollowerItems.aspx.cs" Inherits="DiabloIIIApi.D3FollowerItems" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>D3 Follower Items</title>
    <link href="css/default.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="script/jquery-1.7.1.min.js"></script>
    <script src="script/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="script/json2.js"></script>
    <script src="http://us.battle.net/d3/static/js/tooltips.js"></script>
    <script src="script/d3.js"></script>
    <script src="script/jquery.blockUI.js"></script>
</head>
<body>
            
    <form id="form1" runat="server">
        
    <div id="dialog-hero" title="Your Hero Name Here">
        <div id="tabs">
          <ul>
            <li><a href="#tabs-1">Base</a></li>
            <li><a href="#tabs-2">Offense</a></li>
            <li><a href="#tabs-3">Defense</a></li>
          </ul>
          <div id="tabs-1">
              <table width="100%">
                  <tr><td>Class:</td><td class="value"><label id="class"></label></td></tr>
                  <tr><td>Paragon Level:</td><td class="value"><label id="paragonLevel"></label></td></tr>
                  <tr><td>Dexterity:</td><td class="value"><label id="dexterity"></label></td></tr>
                  <tr><td>Intelligence:</td><td class="value"><label id="intelligence"></label></td></tr>
                  <tr><td>Strength:</td><td class="value"><label id="strength"></label></td></tr>
                  <tr><td>Vitality:</td><td class="value"><label id="vitality"></label></td></tr>
                  <tr><td>Life:</td><td class="value"><label id="life"></label></td></tr>
                  <tr><td>Primary Resource:</td><td class="value"><label id="primaryResource"></label></td></tr>
                  <tr><td>Secondary Resource:</td><td class="value"><label id="secondaryResource"></label></td></tr>
                  <tr><td>Cooldown:</td><td class="value"><label id="cooldown"></label></td></tr>
                  <tr><td>Life per Hit:</td><td class="value"><label id="lifeOnHit"></label></td></tr>
                  <tr><td>Life per Kill:</td><td class="value"><label id="lifePerKill"></label></td></tr>
                  <tr><td>Gold Find:</td><td class="value"><label id="goldFind"></label></td></tr>
                  <tr><td>Magic Find:</td><td class="value"><label id="magicFind"></label></td></tr>
              </table>
          </div>
          <div id="tabs-2">
              <table width="100%">
                  <tr><td>Damage:</td><td class="value"><label id="damage"></label></td></tr>
                  <tr><td>Attack Speed:</td><td class="value"><label id="attackSpeed"></label></td></tr>
                  <tr><td>Critical Hit Chance:</td><td class="value"><label id="critChance"></label></td></tr>
                  <tr><td>Critical Hit Damage:</td><td class="value"><label id="critDamage"></label></td></tr>
                  <tr><td>Thorns:</td><td class="value"><label id="thorns"></label></td></tr>
              </table>
          </div>
          <div id="tabs-3">
              <table width="100%">
                  <tr><td>Arcane Resist:</td><td class="value"><label id="arcaneResit"></label></td></tr>
                  <tr><td>Cold Resist:</td><td class="value"><label id="coldResit"></label></td></tr>
                  <tr><td>Physical Resist:</td><td class="value"><label id="physicalResist"></label></td></tr>
                  <tr><td>Fire Resist:</td><td class="value"><label id="fireResit"></label></td></tr>
                  <tr><td>Lightning Resist:</td><td class="value"><label id="lightningResit"></label></td></tr>
                  <tr><td>Armor:</td><td class="value"><label id="armor"></label></td></tr>
                  <tr><td>Block Chance:</td><td class="value"><label id="blockChance"></label></td></tr>
                  <tr><td>Block Amount:</td><td class="value"><label id="blockAmount"></label></td></tr>
                  <tr><td>Toughness:</td><td class="value"><label id="toughness"></label></td></tr>
              </table>
          </div>
        </div>
    </div>
 
    <h2>Diablo 3 Follower Items</h2>
    Battletag: <input type="text" runat="server" ID="txtBattleTag" placeholder="tagname#1234"></input> 
    Display Followers: <select runat="server" ID="selFollower">
                           <option value="All">All</option>
                           <option value="Enchantress">Enchantress</option>
                           <option value="Scoundrel">Scoundrel</option>
                           <option value="Templar">Templar</option>
                       </select>
    <asp:Button runat="server" Text="Get Follower Items" ID="btnSearch"/>
    <br/>
    <label runat="server" ID="lblError" class="error" Visible="False">BattleTag not found or is incorrectly formatted (tagname#1234) or the service is down.</label>
    <div>
        <table runat="server" ID="tableHero">
            <tr>
                <th>Hero</th>
                <th>Follower</th>
                <th>Special</th>
                <th>Primary</th>
                <th>Off-Hand</th>
                <th>Left Finger</th>
                <th>Right Finger</th>
                <th>Neck</th>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
