# Biblioteca

Projeto final desenvolvido para as unidades UC00607 e UC00608 do curso CET 108 - TPSI.

## Objetivo do projeto

O objetivo deste projeto foi criar um sistema de gestão de biblioteca com um cliente WPF, uma Web API e uma base de dados SQL Server, todos ligados entre si.

A aplicação permite gerir categorias, livros, utilizadores, empréstimos, reservas e penalizações através de uma interface desktop.

## Autor

Alexandre Bezverkhyy

## Estrutura da solução

A solução está dividida em dois projetos:

- Biblioteca.API
- Biblioteca.Cliente.WPF

O cliente WPF não acede diretamente à base de dados. A comunicação é realizada através da Web API.

## Tecnologias utilizadas

- C#
- WPF
- ASP.NET Web API
- .NET Framework 4.8
- .NET 10
- LINQ to SQL
- SQL Server
- Newtonsoft.Json

## Funcionalidades implementadas

### Categorias

- Registar categorias
- Alterar categorias
- Eliminar categorias
- Consultar categorias

### Livros

- Registar livros
- Alterar livros
- Eliminar livros
- Consultar livros
- Pesquisar livros
- Filtrar por categoria e estado
- Controlar o número de exemplares disponíveis

### Utilizadores

- Registar utilizadores
- Alterar utilizadores
- Eliminar utilizadores
- Consultar utilizadores
- Pesquisar utilizadores
- Definir limites de empréstimos
- Registar o número de atrasos
- Bloquear temporariamente utilizadores em caso de atrasos excessivos

### Empréstimos

- Registar empréstimos
- Registar devoluções
- Atualizar automaticamente os exemplares disponíveis
- Aplicar prazos diferentes conforme o tipo de utilizador
- Impedir empréstimos quando o utilizador se encontra bloqueado
- Impedir empréstimos quando existem penalizações não pagas
- Verificar limites de empréstimos

### Reservas

- Registar reservas quando não existem exemplares disponíveis
- Gerir a fila de espera
- Definir a ordem das reservas
- Cancelar reservas
- Eliminar reservas canceladas
- Mostrar o estado da reserva
- Indicar quando o livro está disponível
- Mostrar a data limite para levantamento
- Expirar automaticamente a disponibilidade após três dias
- Passar a disponibilidade para o próximo utilizador da fila

### Penalizações

- Criar penalizações automaticamente em caso de atraso
- Registar penalizações manualmente
- Calcular o valor da multa conforme os dias de atraso
- Registar pagamentos
- Mostrar a data de pagamento
- Bloquear temporariamente utilizadores com atrasos excessivos

### Interface

- Menu lateral de navegação
- Identificação visual da página selecionada
- Página inicial
- Página de créditos
- Botão para sair da aplicação
- Mensagens de validação e confirmação

## Publicação online

A Web API e a base de dados foram publicadas online num ambiente de alojamento temporário para efeitos de demonstração e avaliação.

Endereço base da API:

http://alexb04-001-site1.htempurl.com/

Principais endpoints:

- api/categorias
- api/livros
- api/utilizadores
- api/emprestimos
- api/reservas
- api/penalizacoes

## Como executar

1. Abrir a solução no Visual Studio.
2. Definir Biblioteca.Cliente.WPF como projeto de arranque.
3. Executar a aplicação.
4. Ter ligação à Internet para comunicar com a API e com a base de dados online.

## Testes realizados

Foram testadas as operações de consulta, criação, alteração e eliminação dos dados.

Também foram testadas diferentes validações e situações de erro, como:

- Utilizadores bloqueados devido a atrasos
- Utilizadores com penalizações não pagas
- Livros sem exemplares disponíveis
- Reservas repetidas
- Limites de empréstimos
- Tentativas de realizar operações sem preencher os campos necessários
- Tentativas de eliminar dados que ainda possuem relações com outros registos

Durante os testes, tentei utilizar a aplicação como se fosse um utilizador distraído ou com pressa, para verificar se o sistema apresentava mensagens claras e impedia operações incorretas.

Também foram testadas a criação e a eliminação de dados através do cliente WPF, confirmando depois os resultados diretamente na base de dados online.

## Arquitetura

A arquitetura do projeto pode ser comparada à preparação de uma receita, em que cada parte depende das anteriores.

Primeiro são criadas as categorias. Depois podem ser registados os livros e os utilizadores. Com esses dados já é possível realizar empréstimos, reservas e devoluções.

As penalizações podem ser criadas automaticamente quando um livro é devolvido fora do prazo ou registadas manualmente quando necessário.

Desta forma, cada módulo está ligado aos restantes e contribui para o funcionamento completo do sistema.

O cliente WPF envia os pedidos para a Web API. A API recebe esses pedidos, aplica as regras necessárias e consulta ou altera os dados armazenados no SQL Server.

A comunicação principal do sistema pode ser representada da seguinte forma:

WPF -> Web API -> SQL Server

## Reflexão pessoal

Para mim, a parte mais importante deste projeto foi perceber como uma aplicação pequena pode crescer progressivamente e transformar-se num sistema mais completo.

O projeto começou com entidades e operações simples, mas foi ganhando novas funcionalidades, regras, validações e ligações entre diferentes módulos.

Também utilizei aplicações conhecidas, como a Steam, como referência para observar a organização das interfaces, perceber o que poderia estar em falta e comparar diferentes formas de apresentar informações ao utilizador.

Ao longo do desenvolvimento, foi importante testar os módulos separadamente e depois verificar o funcionamento de todo o sistema em conjunto.

## Versão

2.0

## Data

Agosto de 2026