using CompilerDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompilerDemo.Model
{
    public class Parser
    {
        private List<ParsingError> Errors = new List<ParsingError>();
        private int errorNumber;

        public List<ParsingError> Parse(ObservableCollection<TokenViewModel> tokenViewModels)
        {
            Errors = new List<ParsingError>();
            errorNumber = 1;
            Stack<TokenViewModel> stack = new Stack<TokenViewModel>();
            var openComplex = false;
            bool inComplexNumber = false;
            bool expectingComma = false;
            bool expectingComplex = false;
            bool expectingNumber = false;
            bool expectingJ = false;

            if (tokenViewModels.Count == 0)
            {
                Errors.Add(new ParsingError
                {
                    NumberOfError = errorNumber,
                    Message = "Отсутствует какой-либо токен",
                    StartPos = 0,
                    EndPos = 0,
                    ExpectedToken = ""
                });

                return Errors;
            }

            foreach (var token in tokenViewModels)
            {
                if (!inComplexNumber)
                {
                    if (!IsValidIdentifier(token.RawToken))
                    {
                        Errors.Add(new ParsingError
                        {
                            NumberOfError = errorNumber,
                            Message = "Идентификатор должен начинаться с буквы",
                            StartPos = int.Parse(token.StartPos),
                            EndPos = int.Parse(token.EndPos),
                            ExpectedToken = "Начальный идентификатор"
                        });
                        errorNumber++;
                        continue;
                    }

                    inComplexNumber = true;
                }

                switch (token.RawToken)
                {
                    case "=":
                        if (openComplex)
                        {
                            Errors.Add(new ParsingError
                            {
                                NumberOfError = errorNumber,
                                Message = "Неверный синтаксис комплексного числа",
                                StartPos = int.Parse(token.StartPos),
                                EndPos = int.Parse(token.EndPos),
                                ExpectedToken = ""
                            });
                            errorNumber++;
                        }
                        else
                        {
                            openComplex = true;
                            expectingComplex = true;
                        }
                        break;
                    case "complex":
                        if (openComplex && expectingComplex)
                        {
                            stack.Push(token);
                            expectingComplex = false;
                            expectingNumber = true;
                        }
                        else
                        {
                            Errors.Add(new ParsingError
                            {
                                NumberOfError = errorNumber,
                                Message = "Ожидается знак равно перед complex",
                                StartPos = int.Parse(token.StartPos),
                                EndPos = int.Parse(token.EndPos),
                                ExpectedToken = "="
                            });
                            errorNumber++;
                        }
                        break;
                    case "(":
                        if (openComplex && expectingNumber)
                        {
                            stack.Push(token);
                            expectingNumber = false;
                            expectingJ = true;
                        }
                        else if (openComplex && expectingJ)
                        {
                            Errors.Add(new ParsingError
                            {
                                NumberOfError = errorNumber,
                                Message = "Ожидается число перед скобкой",
                                StartPos = int.Parse(token.StartPos),
                                EndPos = int.Parse(token.EndPos),
                                ExpectedToken = "Number"
                            });
                            errorNumber++;
                        }
                        else
                        {
                            Errors.Add(new ParsingError
                            {
                                NumberOfError = errorNumber,
                                Message = "Ожидается число перед скобкой",
                                StartPos = int.Parse(token.StartPos),
                                EndPos = int.Parse(token.EndPos),
                                ExpectedToken = "Number"
                            });
                            errorNumber++;
                        }
                        break;
                    case ")":
                        if (openComplex && expectingJ)
                        {
                            stack.Push(token);
                            expectingJ = false;
                            expectingComma = true;
                        }
                        else if (openComplex && expectingComma)
                        {
                            expectingComma = false;
                            expectingNumber = true;
                        }
                        else
                        {
                            Errors.Add(new ParsingError
                            {
                                NumberOfError = errorNumber,
                                Message = "Ожидается 'j' перед скобкой",
                                StartPos = int.Parse(token.StartPos),
                                EndPos = int.Parse(token.EndPos),
                                ExpectedToken = "j"
                            });
                            errorNumber++;
                        }
                        break;
                    case ",":
                        if (openComplex && expectingComma)
                        {
                            expectingComma = false;
                            expectingNumber = true;
                        }
                        else if (openComplex && !expectingComma)
                        {
                            Errors.Add(new ParsingError
                            {
                                NumberOfError = errorNumber,
                                Message = "Ожидается запятая между аргументами комплексного числа",
                                StartPos = int.Parse(token.StartPos),
                                EndPos = int.Parse(token.EndPos),
                                ExpectedToken = ","
                            });
                            errorNumber++;
                        }
                        break;
                    default:
                        if (openComplex && expectingNumber && token.RawToken != ",")
                        {
                            if (IsNumber(token.RawToken))
                            {
                                stack.Push(token);
                                expectingNumber = false;
                                expectingJ = true;
                            }
                            else
                            {
                                Errors.Add(new ParsingError
                                {
                                    NumberOfError = errorNumber,
                                    Message = "Ожидается число или 'j' в комплексном числе",
                                    StartPos = int.Parse(token.StartPos),
                                    EndPos = int.Parse(token.EndPos),
                                    ExpectedToken = "Number or 'j'"
                                });
                                errorNumber++;
                            }
                        }
                        else if (openComplex && expectingJ && token.RawToken != ")")
                        {
                            if (token.RawToken == "j")
                            {
                                stack.Push(token);
                                expectingJ = false;
                                expectingComma = true;
                            }
                            else
                            {
                                Errors.Add(new ParsingError
                                {
                                    NumberOfError = errorNumber,
                                    Message = "Ожидается 'j' в комплексном числе",
                                    StartPos = int.Parse(token.StartPos),
                                    EndPos = int.Parse(token.EndPos),
                                    ExpectedToken = "j"
                                });
                                errorNumber++;
                            }
                        }
                        break;
                }

                if (openComplex && stack.Count == 5 && token.RawToken != "(" && token.RawToken != ")")
                {
                    expectingComma = !expectingComma;
                }
            }


            return Errors;
        }

        private bool IsValidIdentifier(string token)
        {
            return token.All(char.IsLetterOrDigit) && char.IsLetter(token[0]);
        }

        private bool IsNumber(string token)
        {
            return double.TryParse(token, out _);
        }
    }
}