using System.Drawing;
using ERP.Services.API.Entities;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Image = QuestPDF.Infrastructure.Image;

namespace ERP.Services.API.Utils

{
    public class QuotationDocument(QuotationEntity entity) : IDocument
    {
        private readonly QuotationEntity _entity = entity;

        public static Image LogoImage { get; } = Image.FromFile("logo.jpg");

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
                    row.ConstantItem(150);
                    row.RelativeItem().AlignCenter().Column(column =>
                    {
                        column
                            .Item()
                            .Text(
                                $"Hotline: 08 3096 5557/ 08 3096 5558/ 09 5785 2021")
                            .FontFamily("Prompt")
                            .FontSize(6)
                            .Bold()
                            .FontColor(Colors.Black);
                    });

                    row.RelativeItem().AlignRight().Text(text =>
                    {
                        text.CurrentPageNumber()
                            .FontFamily("Prompt");
                        text.Span(" / ")
                            .FontFamily("Prompt");
                        text.TotalPages()
                            .FontFamily("Prompt");
                    });
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
                                $"{entity.Customer.DisplayName}")
                            .FontFamily("Prompt")
                            .FontSize(10)
                            .Bold()
                            .FontColor(Colors.Black);

                        column.Spacing(5);

                        column.Item()
                            .Text(text =>
                            {
                                text.Span(
                                        $"{entity.Customer.Address()}")
                                    .FontFamily("Prompt")
                                    .FontSize(6)
                                    ;
                            });

                        column.Item().Text(text =>
                        {
                            text.Span($"โทร: {entity.CustomerContact.TelNo} E-mail: {entity.CustomerContact.Email}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                ;
                            ;
                        });
                    });
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Image(LogoImage);
                        column.Item().AlignRight().Text(text =>
                        {
                            text.Span($"(เลขประจำต\u0e31วผ\u0e39\u0e49เส\u0e35ยภาษ\u0e35 {entity.Customer.TaxId})")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black);
                            ;
                        });
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
                        column.Item().Element(ComponentTable);
                        column.Item().Element(ComponentSum);
                        column.Item().Element(ComponentBank);
                        column.Item().Element(ComponentSignature);
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
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().AlignRight().Text(text =>
                            {
                                text.Span("ใบเสนอราคา / QUOTATION")
                                    .FontFamily("Prompt")
                                    .FontSize(10)
                                    .FontColor(Colors.Blue.Darken4);
                                ;
                            });
                        });
                    });

                    column.Spacing(20);
                    column.Item().Row(column =>
                    {
                        column.RelativeItem().Column(column =>
                        {
                            column.Item().Text(text =>
                            {
                                text.Span("เร\u0e35ยน/ATTN:")
                                    .FontFamily("Prompt")
                                    .FontSize(10)
                                    .Bold()
                                    .FontColor(Colors.Black);
                                column.Spacing(5);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span(
                                    $"{entity.CustomerContact.DisplayName()}")
                                    .FontFamily("Prompt")
                                    .FontSize(6);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span(
                                        $"{entity.Customer.Address()}")
                                    .FontFamily("Prompt")
                                    .FontSize(6);
                            });
                        });

                        column.ConstantItem(70);

                        column.RelativeItem().AlignRight().Column(column =>
                        {
                            column.Item().AlignRight().Text(text =>
                            {
                                text.Span($"เลขท\u0e35\u0e48: {entity.QuotationNo ?? ""}")
                                    .FontFamily("Prompt")
                                    .FontSize(10)
                                    .Bold()
                                    .FontColor(Colors.Black);
                                column.Spacing(5);
                            });
                            column.Item().AlignRight().Text(text =>
                            {
                                text.Span($"ว\u0e31นท\u0e35\u0e48: {entity.QuotationDateTime:dd-MM-yyyy}")
                                    .FontFamily("Prompt")
                                    .FontSize(10)
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
                    row.ConstantItem(300);
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span("กำหนดส\u0e48งส\u0e34นค\u0e49า (DELIVERY DATE)")
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
                            text.Span("การร\u0e31บประก\u0e31น (WARRANTY)")
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
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span(
                                    $"บร\u0e34ษ\u0e31ทฯม\u0e35ความย\u0e34นด\u0e35เสนอราคาเพ\u0e37\u0e48อให\u0e49ท\u0e48านพ\u0e34จารณาด\u0e31งน\u0e35\u0e49 / We are pleased to submit you our quotation as follows")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });

                    row.ConstantItem(165);

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignCenter().Padding(3).Text(text =>
                        {
                            text.Span($"{entity.Projects.FirstOrDefault()?.PaymentCondition.ConditionDescription}")
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
                            text.Span($"{entity.Projects.FirstOrDefault()?.Warranty}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                            ;
                        });
                    });
                });
            }

            void ComponentTable(IContainer container)
            {
                var headerStyle = TextStyle.Default.FontSize(8).SemiBold();


                container.Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().ColumnSpan(7).PaddingTop(5).PaddingBottom(5).BorderBottom(2)
                            .BorderColor(Colors.Blue.Darken4);

                        header.Cell().Text("#")
                                .FontFamily("Prompt");
                        header.Cell().Text("รห\u0e31สส\u0e34นค\u0e49า\n(PRODUCT ID.)").Style(headerStyle)
                                .FontFamily("Prompt");
                        header.Cell().AlignCenter().Text("รายการส\u0e34นค\u0e49า\n(PRODUCT DESCRIPTION)")
                                .FontFamily("Prompt")
                            .Style(headerStyle);
                        header.Cell().AlignCenter().Text("หน\u0e48วย\n(UNIT)").Style(headerStyle)
                                .FontFamily("Prompt");
                        header.Cell().AlignCenter().Text("จำนวน\n(QTY.)").Style(headerStyle)
                                .FontFamily("Prompt");
                        header.Cell().AlignCenter().Text("ราคา/หน\u0e48วย\n(UNIT/PRICE)").Style(headerStyle)
                                .FontFamily("Prompt");
                        header.Cell().AlignCenter().Text("จำนวนเง\u0e34น\n(AMOUNT)").Style(headerStyle)
                                .FontFamily("Prompt");


                        header.Cell().ColumnSpan(7).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Blue.Darken4);
                    });

                    var i = 1;
                    foreach (var product in entity.Products)
                    {
                        table.Cell().Element(CellStyle).Text(i++)
                                .FontFamily("Prompt").FontSize(6);
                        table.Cell().Element(CellStyle).Text(product.Product.ProductCustomId)
                                .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignLeft().Text(product.Product.ProductName)
                                .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text("E.A")
                                .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text(product.Quantity)
                                .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text(product.Product.LwPrice)
                                .FontFamily("Prompt").FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text(product.Amount)
                                .FontFamily("Prompt").FontSize(8);

                        static IContainer CellStyle(IContainer container) => container
                            .PaddingVertical(5);
                    }
                });
            }

            void ComponentSum(IContainer container)
            {
                container.PaddingTop(40).Row(row =>
                {
                    row.Spacing(20);

                    row.ConstantItem(300);

                    row.RelativeItem().AlignRight().Column(column =>
                    {
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"รวมเง\u0e34น (TOTAL)")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"ส\u0e48วนลด (DISCOUNT)")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"ม\u0e39ลค\u0e48าส\u0e34นค\u0e49า/บร\u0e34การ (SUB TOTAL)")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"ภาษ\u0e35ม\u0e39ลค\u0e48าเพ\u0e34\u0e48ม (VAT) 7%")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"รวมท\u0e31\u0e49งหมด (GRAND TOTAL)")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                    });

                    row.RelativeItem().AlignRight().Column(column =>
                    {
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"{entity.Price:#,###:##}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"{entity.SumOfDiscount:#,###:##}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"{entity.AmountBeforeVat:#,###:##}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"{entity.Vat:#,###:##}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                        column.Item().AlignRight().Padding(3).Text(text =>
                        {
                            text.Span($"{(entity.Amount + entity.Vat):#,###:##}")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                ;
                        });
                    });
                });
            }

            void ComponentBank(IContainer container)
            {
                container.Padding(10).Row(row =>
                {
                    row.RelativeItem().Background(Colors.Blue.Darken4).Column(column =>
                    {
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"KBANK (พ\u0e31ฒนาการ 29)")
                                .FontFamily("Prompt")
                                .FontSize(10)
                                .FontColor(Colors.White)
                                ;
                        });
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span(
                                    $"บร\u0e34ษ\u0e31ท ซ\u0e35เค\u0e35ยว โซล\u0e39ช\u0e31\u0e48น เอเซ\u0e35ย จำก\u0e31ด")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.White)
                                ;
                        });
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"บ\u0e31ญช\u0e35: กระแสรายว\u0e31น เลขท\u0e35\u0e48: 727-1-02814-2")
                                .FontFamily("Prompt")
                                .FontSize(8)
                                .FontColor(Colors.White)
                                ;
                        });
                    });

                    row.ConstantItem(30);

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().LineHorizontal(1).LineColor(Colors.Blue.Darken4);
                    });
                });
            }

            void ComponentSignature(IContainer container)
            {
                container.PaddingTop(-60).Row(row =>
                {
                    row.ConstantItem(300);
                    row.RelativeItem().AlignCenter().Column(column =>
                    {
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"ส\u0e31\u0e48งซ\u0e37\u0e49อโดย / ORDERED by")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                        });

                        column.Item().PaddingTop(50).LineHorizontal(1);
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{entity.SalePerson.DisplayNameTH()}")
                                .FontFamily("Prompt")
                                .FontSize(8)
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
                            text.Span($"{entity.SalePerson.TelNo}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{entity.SalePerson.email}")
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
                        column.Item().Padding(3).Text(text =>
                        {
                            text.Span($"จ\u0e31ดทำโดย / ARRANGED by")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .FontColor(Colors.Black)
                                ;
                        });

                        column.Item().PaddingTop(50).LineHorizontal(1);
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{entity.IssuedByUser.DisplayNameTH()}")
                                .FontFamily("Prompt")
                                .FontSize(8)
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
                            text.Span($"{entity.IssuedByUser.TelNo}")
                                .FontFamily("Prompt")
                                .FontSize(6)
                                .SemiBold()
                                .FontColor(Colors.Black)
                                .LineHeight(1)
                                ;
                        });
                        column.Item().Padding(3).AlignCenter().Text(text =>
                        {
                            text.Span($"{entity.IssuedByUser.email}")
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