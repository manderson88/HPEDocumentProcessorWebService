DWG.aspx
<%@ Page Language="C#" %>
<script runat="server">
   void runSrvice_Click(Object sender, EventArgs e){
      DWGTranslatorService mySvc = new DWGTranslatorService();
      Label1.Text = mySvc.SayHello();
      Label2.Text = mySvc.Add(Int32.Parse(txtNum1.Text),  Int32.Parse(txtNum2.Text)).ToString();
   }
</script>

<html>
   <head> </head>

   <body>
      <form runat="server">
         <p>
            <em>First Number to Add </em>:
            <asp:TextBox id="txtNum1" runat="server" />
         </p>

         <p>
            <em>Second Number To Add </em>:
            <asp:TextBox id="txtNum2" runat="server" />
         </p>

         <p>
            <strong><u>Web Service Result -</u></strong>
         </p>

         <p>
            <em>Hello world Service</em> :
            <asp:Label id="Label1" runat="server" Font-Underline="True">Label</asp:Label>
         </p>

         <p>
            <em>Add Service</em> :
            & <asp:Label id="Label2" runat="server" Font-Underline="True">Label</asp:Label>
         </p>

         <p align="left">
            <asp:Button id="runSrvice" onclick="runSrvice_Click" runat="server"  Text="Execute"></asp:Button>
         </p>

      </form>

   </body>
</html>
