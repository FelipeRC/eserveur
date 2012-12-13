﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Softex.Residencia.EServeur.Business;
using Softex.Residencia.EServeur.Model;
using System.IO;
using System.Drawing.Imaging;

namespace Softex.Residencia.Projeto.UI.Administrator
{
    public partial class FrmCadastroProduto : Form
    {
        private readonly IngredienteBusiness ingredienteBusiness;
        private readonly ProdutoBusiness produtoBusiness;

        public FrmCadastroProduto()
        {
            InitializeComponent();

            this.ingredienteBusiness = new IngredienteBusiness();
            this.produtoBusiness = new ProdutoBusiness();

            this.CarregarIngredientes();
        }

        private void btnCadastrarIngrediente_Click(object sender, EventArgs e)
        {
            string nome = this.txtNomeIngrediente.Text.Trim();
            decimal preco = 0;

            if (!decimal.TryParse(this.txtPrecoIngrediente.Text, out preco))
            {
                throw new Exception(Mensagens.PrecoIngredienteInvalido);
            }

            Ingrediente ingrediente = new Ingrediente()
            {
                Nome = nome,
                Preco = preco
            };

            this.ingredienteBusiness.CadastrarIngrediente(ingrediente);
            this.LimparCamposGroupBoxIngrediente();
            this.CarregarIngredientes();

            MessageBox.Show(Mensagens.IngredienteCadastrado, Mensagens.Mensagem, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void LimparCamposGroupBoxIngrediente()
        {
            this.txtNomeIngrediente.Text = "";
            this.txtPrecoIngrediente.Text = "";
            this.chkDisponivelEmEstoque.Checked = true;
        }

        private void CarregarIngredientes()
        {
            this.cboIngredientes.DataSource = this.ingredienteBusiness.RecuperarNomesIngredientes();
        }

        private void btnAdicionarImagemPrato_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.picImagemPrato.Image = new Bitmap(this.openFileDialog.FileName);
            }
        }

        private void btnSalvarPrato_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria()
                                      {
                                          Nome = "Pratos"
                                      };

            Produto produto = new Produto()
            {
                Nome = this.txtNomePrato.Text,
                Descricao = this.txtDescricaoPrato.Text,
                Preco = Convert.ToDecimal(this.txtPrecoPrato.Text),
                Categoria = categoria
            };

            using (MemoryStream ms = new MemoryStream())
            {
                Image image = this.picImagemPrato.Image;
                image.Save(ms, ImageFormat.Png);

                produto.Imagem = ms.ToArray();
            }

            this.produtoBusiness.CadastrarProduto(produto);
        }
    }
}