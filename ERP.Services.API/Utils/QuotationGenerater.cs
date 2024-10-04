using System.Drawing;
using ERP.Services.API.Entities;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Image = QuestPDF.Infrastructure.Image;

namespace ERP.Services.API.Utils

{
    public class QuotationDocument : IDocument
    {
        private readonly QuotationEntity _entity;
        private readonly BusinessEntity _business;
        private readonly string _businessAddress;
        private readonly string _customerAddress;
        public string FileName { get; set; }

        public QuotationDocument(QuotationEntity entity, BusinessEntity business, string businessAddress,
            string customerAddress)
        {
            _entity = entity;
            _business = business;
            _businessAddress = businessAddress;
            _customerAddress = customerAddress;
            FileName = entity.QuotationNo;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            FontManager.RegisterFont(File.OpenRead(Path.Combine("Fonts", "Prompt.ttf")));
            container
                .Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);
                    page.Header().Element(ComposeHeader);

                    page.Content().Element(ComposeContent);

                    page.Footer().Element(ComposeFooter);
                });

            void ComposeFooter(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text(text =>
                    {
                        text.Span("Hotline : " + _entity.Business.Hotline)
                            .FontSize(8)
                            .FontColor(Colors.Black)
                            .FontFamily("Prompt");
                        text.Span("    " + _entity.Business.Url)
                            .FontSize(8)
                            .FontColor(Colors.Black)
                            .FontFamily("Prompt");

                        text.Span("            ")
                            .FontSize(8)
                            .FontColor(Colors.Black)
                            .FontFamily("Prompt");

                        text.Span(_entity.QuotationNo)
                            .FontSize(8)
                            .FontColor(Colors.Black)
                            .FontFamily("Prompt");
                        text.Span("    หน้าที่ :")
                            .FontSize(8)
                            .FontFamily("Prompt");
                        text.CurrentPageNumber()
                            .FontSize(8)
                            .FontFamily("Prompt");
                        text.Span(" / ")
                            .FontSize(8)
                            .FontFamily("Prompt");
                        text.TotalPages()
                            .FontSize(8)
                            .FontFamily("Prompt");
                    });
                    // row.RelativeItem().AlignCenter().Text(text =>
                    // {
                    //     text.Span(_entity.Business.Url)
                    //         .FontSize(8)
                    //         .FontColor(Colors.Black)
                    //         .FontFamily("Prompt");
                    // });
                    // row.RelativeItem().AlignRight().Text(text =>
                    // {
                    //     text.Span(_entity.QuotationNo)
                    //         .FontSize(8)
                    //         .FontColor(Colors.Black)
                    //         .FontFamily("Prompt");
                    //     text.Span("    หน้าที่ :")
                    //         .FontSize(8)
                    //         .FontFamily("Prompt");
                    //     text.CurrentPageNumber()
                    //         .FontSize(8)
                    //         .FontFamily("Prompt");
                    //     text.Span(" / ")
                    //         .FontSize(8)
                    //         .FontFamily("Prompt");
                    //     text.TotalPages()
                    //         .FontSize(8)
                    //         .FontFamily("Prompt");
                    // });
                });
            }

            void ComposeHeader(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column
                            .Item()
                            .Text(
                                $"{_business.BusinessName} {(_business.BrnId == "00000" ? "(สำน\u0e31กงานใหญ\u0e48)" : "")}")
                            .FontFamily("Prompt")
                            .FontSize(10)
                            .Bold()
                            .FontColor(Colors.Black);

                        column.Spacing(2);

                        column.Item()
                            .Text(text =>
                            {
                                text.Span(
                                        $"{_businessAddress}")
                                    .FontFamily("Prompt")
                                    .FontSize(6)
                                    ;
                            });

                        column.Item()
                            .Text(text =>
                            {
                                text.Span(
                                        $"โทรศัพท์ : {_entity.Business.Tel}")
                                    .FontFamily("Prompt")
                                    .FontSize(6)
                                    ;
                            });
                        column.Item()
                            .Text(text =>
                            {
                                text.Span(
                                        $"E-Mail : {_entity.Business.Email}")
                                    .FontFamily("Prompt")
                                    .FontSize(6)
                                    ;
                            });

                        column.Item().Text(text =>
                        {
                            text.Span($"(เลขประจำต\u0e31วผ\u0e39\u0e49เส\u0e35ยภาษ\u0e35 {_business.TaxId})")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black);
                            ;
                        });

                        // column.Item().Text(text =>
                        // {
                        //     text.Span($"โทร: {business.} E-mail: {entity.CustomerContact.Email}")
                        //         .FontFamily("Prompt")
                        //         .FontSize(6)
                        //         ;
                        //     ;
                        // });
                    });


                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Image(Image.FromFile($"images/{_entity.Business.Logo}"));
                    });
                });
            }

            void ComposeContent(IContainer container)
            {
                container.PaddingVertical(15).Column(column =>
                    {
                        column.Item().Element(ComponentNameSpace);
                        column.Item().Element(ComponentWarrantyHeader);
                        column.Item().Element(ComponentWarrantyDetail);
                        column.Item().Element(ComponentRemark);
                        column.Item().Element(ComponentTable);
                        column.Item().Element(ComponentLine);
                        column.Item().Element(ComponentBank);
                        column.Item().Element(ComponentSum);
                        column.Item().Element(ComponentLineSum);
                        column.Item().Element(ComponentSignature);
                        column.Item().Element(ComponentNote);
                        // column.Item().Element(ComponentLineBottom);
                    }
                );
            }

            void ComponentNameSpace(IContainer container)
            {
                container.PaddingVertical(15).Column(column =>
                {
                    column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Image(Image.FromFile($"images/Quotation_Line.png"));
                            });
                        }
                    );


                    column.Spacing(20);
                    column.Item().Row(column =>
                    {
                        column.RelativeItem().Column(column =>
                        {
                            column.Item().Text(text =>
                            {
                                text.Span("รายละเอียดลูกค้า : Customer Description")
                                    .FontFamily("Prompt")
                                    .FontSize(8)
                                    .Bold()
                                    .FontColor(Colors.Black);
                                column.Spacing(3);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span("เร\u0e35ยน/ATTN:")
                                    .FontFamily("Prompt")
                                    .FontSize(8)
                                    .Bold()
                                    .FontColor(Colors.Black);
                                column.Spacing(3);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span(
                                        $"คุณ {_entity.CustomerContact.DisplayName()}")
                                    .FontFamily("Prompt")
                                    .FontSize(8);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span(
                                        $"{_entity.Customer.CusName}")
                                    .FontFamily("Prompt")
                                    .FontSize(8);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span(
                                        $"{_customerAddress}")
                                    .FontFamily("Prompt")
                                    .FontSize(8);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span(
                                        $"โทร: {_entity.CustomerContact.TelNo} E-mail: {_entity.CustomerContact.Email}")
                                    .FontFamily("Prompt")
                                    .FontSize(8)
                                    ;
                                ;
                            });
                        });

                        column.ConstantItem(70);

                        column.RelativeItem().AlignRight().Column(column =>
                        {
                            column.Item().AlignRight().Text(text =>
                            {
                                text.Span($"เลขท\u0e35\u0e48 / Quotation No: {_entity.QuotationNo ?? ""}")
                                    .FontFamily("Prompt")
                                    .FontSize(8)
                                    .Bold()
                                    .FontColor(Colors.Black);
                                column.Spacing(5);
                            });
                            column.Item().AlignRight().Text(text =>
                            {
                                text.Span($"ว\u0e31นท\u0e35\u0e48 / Date: {_entity.QuotationDateTime:dd-MM-yyyy}")
                                    .FontFamily("Prompt")
                                    .FontSize(8)
                                    .FontColor(Colors.Black);
                            });
                        });
                    });
                });
            }

            void ComponentWarrantyHeader(IContainer container)
            {
                container.Row(row =>
                {
                    // row.ConstantItem(300);
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span("ชื่อโครงการ (Project)")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.White)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span("เง\u0e37\u0e48อนไขการชำระเง\u0e34น (Payment Term)")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.White)
                                ;
                            ;
                        });
                    });
                    // row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    // {
                    //     column.Item().AlignCenter().Padding(3).Text(text =>
                    //     {
                    //         text.Span("เง\u0e37\u0e48อนไขการชำระเง\u0e34น")
                    //             .FontFamily("Prompt")
                    //             .FontSize(6)
                    //             .FontColor(Colors.White)
                    //             ;
                    //         ;
                    //     });
                    // });
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span("กำหนดส\u0e48งส\u0e34นค\u0e49า (Delivery Date)")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.White)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span("การร\u0e31บประก\u0e31น (Warranty)")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.White)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span("สกุลเงิน (Currency)")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.White)
                                ;
                            ;
                        });
                    });
                });
            }

            void ComponentWarrantyDetail(IContainer container)
            {
                container.Row(row =>
                {
                    // row.RelativeItem().Column(column =>
                    // {
                    //     column.Item().Padding(3).Text(text =>
                    //     {
                    //         text.Span(
                    //                 $"บร\u0e34ษ\u0e31ทฯม\u0e35ความย\u0e34นด\u0e35เสนอราคาเพ\u0e37\u0e48อให\u0e49ท\u0e48านพ\u0e34จารณาด\u0e31งน\u0e35\u0e49 / We are pleased to submit you our quotation as follows")
                    //             .FontFamily("Prompt")
                    //             .FontSize(6)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //         ;
                    //     });
                    // });

                    // row.ConstantItem(165);
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"{_entity.Projects.FirstOrDefault()?.Project.ProjectName}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"{_entity.Projects.FirstOrDefault()?.PaymentCondition.ConditionDescription}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });

                    row.RelativeItem().Column(column =>
                    {
                        var textEth =
                            _entity.Projects.FirstOrDefault() != null &&
                            _entity.Projects.FirstOrDefault().EthSaleMonth.HasValue
                                ? _entity.Projects.FirstOrDefault().EthSaleMonth.Value.ToString("MM/yyyy")
                                : "-";
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"{textEth}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"{_entity.Projects.FirstOrDefault()?.Warranty}" + " เดือน")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"บาท (THB)")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                });
            }

            void ComponentRemark(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Padding(10).Text(text =>
                        {
                            text.Span(
                                    $"บร\u0e34ษ\u0e31ทฯ ม\u0e35ความย\u0e34นด\u0e35เสนอราคาเพ\u0e37\u0e48อให\u0e49ท\u0e48านพ\u0e34จารณาด\u0e31งน\u0e35\u0e49 / We are pleased to submit you our quotation as follows")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                });
            }

            void ComponentNote(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeItem().PaddingTop(20).Column(column =>
                    {
                        column.Item().Padding(10).Text(text =>
                        {
                            text.Span(
                                    $"หมายเหตุ (Remark) : {_entity.Remark}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                });
            }

            void ComponentTable(IContainer container)
            {
                var headerStyle = TextStyle.Default.FontSize(6).SemiBold();


                container.Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);
                        columns.ConstantColumn(60);
                        columns.RelativeColumn();
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(50);

                        // columns.RelativeColumn();
                        // columns.RelativeColumn();
                        // columns.RelativeColumn();
                        // columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().ColumnSpan(7).PaddingTop(5).PaddingBottom(5).BorderBottom(2)
                            .BorderColor(Colors.Blue.Darken4);

                        header.Cell().Text("#")
                            .FontSize(6)
                            .FontFamily("Prompt");
                        header.Cell().Text("รห\u0e31สส\u0e34นค\u0e49า\n(PRODUCT ID.)")
                            .FontSize(6)
                            .Style(headerStyle)
                            .FontFamily("Prompt");
                        header.Cell().AlignCenter().Text("รายการส\u0e34นค\u0e49า\n(DESCRIPTION)")
                            .FontSize(6)
                            .FontFamily("Prompt")
                            .Style(headerStyle);
                        header.Cell().AlignCenter().Text("หน\u0e48วย\n(UNIT)").Style(headerStyle)
                            .FontSize(6)
                            .FontFamily("Prompt");
                        header.Cell().AlignCenter().Text("จำนวน\n(QTY.)").Style(headerStyle)
                            .FontSize(6)
                            .FontFamily("Prompt");
                        header.Cell().AlignCenter().AlignRight().Text("ราคา/หน\u0e48วย\n(UNIT/PRICE)")
                            .FontSize(6)
                            .Style(headerStyle)
                            .FontFamily("Prompt");
                        header.Cell().AlignCenter().AlignRight().Text("จำนวนเง\u0e34น\n(AMOUNT)").Style(headerStyle)
                            .FontSize(6)
                            .FontFamily("Prompt");


                        header.Cell().ColumnSpan(7).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Blue.Darken4);
                    });

                    var i = 1;
                    foreach (var product in _entity.Products.OrderBy(x => x.Order))
                    {
                        table.Cell().Element(CellStyle).Text(i++)
                            .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).Text(product.Product.ProductCustomId)
                            .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignLeft().Text(product.Product.ProductName)
                            .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignCenter().Text("EA")
                            .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignCenter().Text(product.Quantity)
                            .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text(product.Amount.ToString("#,###.00"))
                            .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text(product.AmountBeforeVat.ToString("#,###.00"))
                            .FontFamily("Prompt").FontSize(8);


                        static IContainer CellStyle(IContainer container) => container
                            .PaddingVertical(8);
                        // .LineHorizontal(Colors.Blue.Darken4);
                    }
                });
            }

            void ComponentLine(IContainer container)
            {
                container.PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                });
            }

            void ComponentLineBottom(IContainer container)
            {
                var padding = 110 - 10 * _entity.Products.Count;
                container.PaddingTop(padding).Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                });
            }


            void ComponentBank(IContainer container)
            {
                container.PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"สามารถชำระเงินผ่านธนาคารในนาม {_entity.Business.BusinessName}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.White)
                                ;
                        });
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span(
                                    $"{_entity.PaymentAccountEntity.BankEntity.BankTHName} {_entity.PaymentAccountEntity.BankBranchEntity.BankBranchTHName}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.White)
                                ;
                        });
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span(
                                    $"บ\u0e31ญช\u0e35: {_entity.PaymentAccountEntity.AccountType} เลขท\u0e35\u0e48: {_entity.PaymentAccountEntity.PaymentAccountNo}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.White)
                                ;
                        });
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"ชื่อบัญชี: {_entity.PaymentAccountEntity.PaymentAccountName}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.White)
                                ;
                        });
                    });

                    row.ConstantItem(300);

                    // row.RelativeItem().AlignRight().PaddingTop(50).Column(column =>
                    // {
                    //     column.Item().AlignRight().PaddingTop(1).Text(text =>
                    //     {
                    //         text.Span($"รวมเง\u0e34น (SUB TOTAL)")
                    //             .FontFamily("Prompt")
                    //             .FontSize(8)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //     });
                    //
                    //     column.Item().AlignRight().PaddingTop(1).Text(text =>
                    //     {
                    //         text.Span($"ภาษ\u0e35ม\u0e39ลค\u0e48าเพ\u0e34\u0e48ม (VAT) 7%")
                    //             .FontFamily("Prompt")
                    //             .FontSize(8)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //     });
                    //     column.Item().AlignRight().PaddingTop(1).Text(text =>
                    //     {
                    //         text.Span($"รวมท\u0e31\u0e49งหมด (GRAND TOTAL)")
                    //             .FontFamily("Prompt")
                    //             .FontSize(8)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //     });
                    // });
                    //
                    // row.RelativeItem().AlignRight().PaddingTop(50).Column(column =>
                    // {
                    //     column.Item().AlignRight().PaddingTop(1).Text(text =>
                    //     {
                    //         text.Span($"{_entity.AmountBeforeVat:#,###.00}")
                    //             .FontFamily("Prompt")
                    //             .FontSize(8)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //     });
                    //     // column.Item().AlignRight().Padding(3).Text(text =>
                    //     // {
                    //     //     text.Span($"{entity.SumOfDiscount:#,###:##}")
                    //     //         .FontFamily("Prompt")
                    //     //         .FontSize(8)
                    //     //         .FontColor(Colors.Black)
                    //     //         ;
                    //     // });
                    //     // column.Item().AlignRight().Padding(3).Text(text =>
                    //     // {
                    //     //     text.Span($"{entity.AmountBeforeVat:#,###:##}")
                    //     //         .FontFamily("Prompt")
                    //     //         .FontSize(8)
                    //     //         .FontColor(Colors.Black)
                    //     //         ;
                    //     // });
                    //     column.Item().AlignRight().PaddingTop(1).Text(text =>
                    //     {
                    //         text.Span($"{_entity.Vat:#,###.00}")
                    //             .FontFamily("Prompt")
                    //             .FontSize(8)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //     });
                    //     column.Item().AlignRight().PaddingTop(1).Text(text =>
                    //     {
                    //         text.Span($"{(_entity.Price + _entity.Vat):#,###.00}")
                    //             .FontFamily("Prompt")
                    //             .FontSize(8)
                    //             .FontColor(Colors.Black)
                    //             ;
                    //     });
                    // });
                });
            }

            void ComponentSum(IContainer container)
            {
                container.PaddingTop(-95).Row(row =>
                {
                    row.ConstantItem(330);

                    row.RelativeItem().AlignRight().PaddingTop(50).Column(column =>
                    {
                        column.Item().AlignRight().PaddingTop(1).Text(text =>
                        {
                            text.Span($"รวมเง\u0e34น (SUB TOTAL)")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });

                        column.Item().AlignRight().PaddingTop(1).Text(text =>
                        {
                            text.Span($"ภาษ\u0e35ม\u0e39ลค\u0e48าเพ\u0e34\u0e48ม (VAT) 7%")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().PaddingTop(1).Text(text =>
                        {
                            text.Span($"รวมท\u0e31\u0e49งหมด (GRAND TOTAL)")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                    });

                    row.RelativeItem().AlignRight().PaddingTop(50).Column(column =>
                    {
                        column.Item().AlignRight().PaddingTop(1).Text(text =>
                        {
                            text.Span($"{_entity.AmountBeforeVat:#,###.00}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        // column.Item().AlignRight().Padding(3).Text(text =>
                        // {
                        //     text.Span($"{entity.SumOfDiscount:#,###:##}")
                        //         .FontFamily("Prompt")
                        //         .FontSize(8)
                        //         .FontColor(Colors.Black)
                        //         ;
                        // });
                        // column.Item().AlignRight().Padding(3).Text(text =>
                        // {
                        //     text.Span($"{entity.AmountBeforeVat:#,###:##}")
                        //         .FontFamily("Prompt")
                        //         .FontSize(8)
                        //         .FontColor(Colors.Black)
                        //         ;
                        // });
                        column.Item().AlignRight().PaddingTop(1).Text(text =>
                        {
                            text.Span($"{_entity.Vat:#,###.00}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().PaddingTop(1).Text(text =>
                        {
                            text.Span($"{(_entity.Price + _entity.Vat):#,###.00}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                    });
                });
            }

            void ComponentLineSum(IContainer container)
            {
                container
                    .PaddingTop(10)
                    .Row(row =>
                    {
                        row.ConstantItem(330);

                        row.RelativeItem().Column(column =>
                        {
                            column.Item()
                                .LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                        });
                    });
            }

            void ComponentSignature(IContainer container)
            {
                container.PaddingTop(20).Row(row =>
                {
                    // row.ConstantItem(300);


                    row.RelativeItem().AlignCenter().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"จ\u0e31ดทำโดย / Arranged by")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                        });

                        column.Item().AlignCenter().Width(100).Height(100)
                            .Image(Image.FromFile($"images/{_entity.SalePersonId}.png"));


                        column.Item().PaddingTop(-23).LineHorizontal(1);
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{_entity.SalePerson.DisplayNameTH()}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });
                        // column.Item().Padding(3).AlignCenter().Text(text =>
                        // {
                        //     text.Span($"(WITCHAYADA APIN)")
                        //         .FontSize(8)
                        //         .SemiBold()
                        //         .FontColor(Colors.Black)
                        //         .LineHeight(1)
                        //         ;
                        // });
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{_entity.SalePerson.TelNo}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });

                        var business = _entity.Business.BusinessName;
                        var email = "";

                        var arrOfEmail = _entity.SalePerson.email?.Split("|");

                        if (business ==
                            "บร\u0e34ษ\u0e31ท แปซ\u0e34ฟ\u0e34ค เทคโนโลย\u0e35 ด\u0e34สตร\u0e34บ\u0e34วช\u0e31\u0e48น จำก\u0e31ด")
                        {
                            email = arrOfEmail?.FirstOrDefault(x => x.Contains("ptdthai"));
                        }
                        else if (business ==
                                 "บร\u0e34ษ\u0e31ท ซ\u0e35เค\u0e35ยว โซล\u0e39ช\u0e31\u0e48น เอเซ\u0e35ย จำก\u0e31ด")
                        {
                            email = arrOfEmail?.FirstOrDefault(x => x.Contains("securesolutionsasia"));
                        }
                        else if (business == "บร\u0e34ษ\u0e31ท ไซเบอร\u0e4c มาสเตอร\u0e4cส จำก\u0e31ด")
                        {
                            email = arrOfEmail?.FirstOrDefault(x => x.Contains("cybermasters"));
                        }

                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{email}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });
                    });
                    row.ConstantItem(30);
                    row.RelativeItem().AlignCenter().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"ข้าพเจ้าตกลงสั่งซื้อตามรายการและเงื่อนไขทั้งหมด")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                        });
                        // column.Item().AlignCenter().Padding(3).Text(text =>
                        // {
                        //     text.Span($"และเงื่อนไขทั้งหมด")
                        //         .FontFamily("Prompt")
                        //         .FontSize(6)
                        //         .FontColor(Colors.Black)
                        //         ;
                        // });

                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"We agree & accept to order you as in this quotation")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                        });


                        column.Item().PaddingTop(61).LineHorizontal(1);
                        // column.Item().Padding(3).AlignCenter().Text(text =>
                        // {
                        //     text.Span($"{entity.SalePerson.DisplayNameTH()}")
                        //         .FontFamily("Prompt")
                        //         .FontSize(8)
                        //         .SemiBold()
                        //         .FontColor(Colors.Black)
                        //         .LineHeight(1)
                        //         ;
                        // });
                        // column.Item().Padding(3).AlignCenter().Text(text =>
                        // {
                        //     text.Span($"(WITCHAYADA APIN)")
                        //         .FontSize(8)
                        //         .SemiBold()
                        //         .FontColor(Colors.Black)
                        //         .LineHeight(1)
                        //         ;
                        // });
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"ลงชื่อลูกค้าพร้อมประทับตรา")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"Customer Signature with Company Seal")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });
                    });
                });
            }
        }
    }
}