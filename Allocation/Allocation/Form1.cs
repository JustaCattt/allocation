using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Allocation
{
    public partial class Form1 : Form
    {
        string[] algorithm = new string[] { "最先适应法", "最佳适应法", "最坏适应法" };
        string[] state = new string[] { "空闲", "被占用" };
        int[] memory = { 64, 128, 256, 512, 1024 };     //初始内存大小
        string[] data;
        string[] allodata;
        public static int clickcount = 0;
        public int now_memory;      //仅剩内存
        public Table[] tables = new Table[100];
        public List<Job> jobs = new List<Job>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)      //添加分区表
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("未选择表的状态属性，无法执行此操作");
            }
            else
            {
                if (Convert.ToInt32(textBox1.Text) <= now_memory)
                {
                    if (textBox6.Text == "")
                    {
                        if (clickcount == 0)
                            tables[clickcount].address = 0;
                        else
                            tables[clickcount].address = tables[clickcount - 1].address + tables[clickcount - 1].length;
                    }
                    else
                        tables[clickcount].address = Convert.ToInt32(textBox6.Text);

                    tables[clickcount].length = Convert.ToInt32(textBox1.Text);

                    tables[clickcount].state = comboBox1.Text;
                    if (comboBox1.Text == "被占用")
                    {
                        if(textBox2.Text!="")
                            tables[clickcount].occupier = textBox2.Text;
                        else
                            MessageBox.Show("请输入占用作业名");
                    }
                    else
                    {
                        tables[clickcount].occupier = "";
                    }

                    tables[clickcount].index++;

                    now_memory -= tables[clickcount].length;
                    textBox3.Text = now_memory.ToString();

                    data = new string[] { tables[clickcount].address.ToString(), tables[clickcount].length.ToString(), tables[clickcount].state, tables[clickcount].occupier };
                    if (data.Length != 0)
                    {
                        ListViewItem lvi = new ListViewItem(data);
                        listView1.Items.Add(lvi);
                        textBox1.Text = "";
                        textBox2.Text = "";
                        comboBox1.Text = "";
                        data = new string[] { };
                    }
                    clickcount++;
                }
                else
                {
                    MessageBox.Show("当前剩余内存不足以完成此操作");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      //释放选中项对应表的内存
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择想要删除的内容");
            }
            else
            {
                now_memory += Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text);
                textBox3.Text = now_memory.ToString();
                List<Table> temp = tables.ToList();
                temp.RemoveAt(listView1.SelectedItems[0].Index);
                tables = temp.ToArray();
                listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
                listView1.SelectedItems.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Add("起始地址", 75, HorizontalAlignment.Center);
            listView1.Columns.Add("长度", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("状态", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("占用作业", 75, HorizontalAlignment.Center);
            listView2.View = View.Details;
            listView2.Columns.Add("作业名", 75, HorizontalAlignment.Center);
            listView2.Columns.Add("作业大小", 75, HorizontalAlignment.Center);
            for (int i = 0; i < state.Length; i++)
            {
                comboBox1.Items.Add(state[i]);
            }
            for (int i = 0; i < memory.Length; i++)
            {
                comboBox2.Items.Add(memory[i]);
            }
            for (int i = 0; i < algorithm.Length; i++)
            {
                comboBox3.Items.Add(algorithm[i]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = comboBox2.Text;
            now_memory = Convert.ToInt32(comboBox2.Text);
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)      //添加作业
        {
            Job job = new Job();
            job.name = textBox4.Text;
            job.length = Convert.ToInt32(textBox5.Text);
            jobs.Add(job);
            data = new string[] { textBox4.Text, textBox5.Text };
            if (data.Length != 0)
            {
                ListViewItem lvi = new ListViewItem(data);
                listView2.Items.Add(lvi);
                textBox4.Text = "";
                textBox5.Text = "";
                data = new string[] { };
            }
        }

        private void ins_sort()         //按照分区长度从小到大排序
        {
            Table temp;
            for (int i = 0; i < clickcount - 1; i++)
                for (int j = 0; j < clickcount - i - 1; j++)
                    if (tables[j].length > tables[j + 1].length)        //将表的信息交换，保留原下标
                    {
                        temp.address = tables[j].address;
                        temp.length = tables[j].length;
                        temp.state = tables[j].state;
                        temp.occupier = tables[j].occupier;

                        tables[j].address = tables[j + 1].address;
                        tables[j].length = tables[j + 1].length;
                        tables[j].state = tables[j + 1].state;
                        tables[j].occupier = tables[j + 1].occupier;

                        tables[j + 1].address = temp.address;
                        tables[j + 1].length = temp.length;
                        tables[j + 1].state = temp.state;
                        tables[j + 1].occupier = temp.occupier;
                    }
        }

        private void des_sort()         //按照分区长度从大到小排序
        {
            Table temp;
            for (int i = 0; i < clickcount - 1; i++)
                for (int j = 0; j < clickcount - i - 1; j++)
                    if (tables[j].length < tables[j + 1].length)        //将表的信息交换，保留原下标
                    {
                        temp.address = tables[j].address;
                        temp.length = tables[j].length;
                        temp.state = tables[j].state;
                        temp.occupier = tables[j].occupier;

                        tables[j].address = tables[j + 1].address;
                        tables[j].length = tables[j + 1].length;
                        tables[j].state = tables[j + 1].state;
                        tables[j].occupier = tables[j + 1].occupier;

                        tables[j + 1].address = temp.address;
                        tables[j + 1].length = temp.length;
                        tables[j + 1].state = temp.state;
                        tables[j + 1].occupier = temp.occupier;
                    }
        }
        private void first_fit()        //最先适应法
        {
            foreach (Job job in jobs)
            {
                for (int j = 0; j < clickcount; j++)
                {
                    if (job.length <= tables[j].length && tables[j].state == "空闲")
                    {
                        List<Table> temp = tables.ToList();
                        Table table = new Table();
                        table.address = tables[j].address + job.length;
                        table.length = tables[j].length - job.length;
                        table.state = "空闲";
                        table.occupier = "";
                        temp.Insert(j + 1, table);
                        tables = temp.ToArray();
                        allodata = new string[] { tables[j + 1].address.ToString(), tables[j + 1].length.ToString(), tables[j + 1].state, tables[j + 1].occupier };

                        tables[j].length = job.length;
                        tables[j].state = "被占用";
                        tables[j].occupier = job.name;
                        data = new string[] { tables[j].address.ToString(), tables[j].length.ToString(), tables[j].state, tables[j].occupier };
                        if (data.Length != 0)
                        {
                            ListViewItem lvi = new ListViewItem(data);
                            listView1.Items.RemoveAt(j);
                            listView1.Items.Insert(j, lvi);
                            lvi = new ListViewItem(allodata);
                            listView1.Items.Insert(j + 1, lvi);
                            listView2.Items.RemoveAt(0);
                            data = new string[] { };
                            allodata = new string[] { };
                        }
                        break;
                    }
                }
            }
        }
        private void best_fit()         //最佳适应法
        {
            ins_sort();
            foreach (Job job in jobs)
            {
                for (int j = 0; j < clickcount; j++)
                {
                    if (job.length <= tables[j].length && tables[j].state == "空闲")
                    {
                        List<Table> temp = tables.ToList();
                        Table table = new Table();
                        table.address = tables[j].address + job.length;
                        table.length = tables[j].length - job.length;
                        table.state = "空闲";
                        table.occupier = "";
                        temp.Insert(j + 1, table);
                        tables = temp.ToArray();
                        allodata = new string[] { tables[j + 1].address.ToString(), tables[j + 1].length.ToString(), tables[j + 1].state, tables[j + 1].occupier };

                        tables[j].length = job.length;
                        tables[j].state = "被占用";
                        tables[j].occupier = job.name;
                        data = new string[] { tables[j].address.ToString(), tables[j].length.ToString(), tables[j].state, tables[j].occupier };
                        if (data.Length != 0)
                        {
                            ListViewItem lvi = new ListViewItem(data);
                            listView1.Items.RemoveAt(tables[j].index+1);
                            listView1.Items.Insert(tables[j].index+1, lvi);
                            lvi = new ListViewItem(allodata);
                            listView1.Items.Insert(tables[j].index+2, lvi);
                            listView2.Items.RemoveAt(0);
                            data = new string[] { };
                            allodata = new string[] { };
                        }
                        break;
                    }
                }
            }
        }
        private void worst_fit()        //最坏适应法
        {
            des_sort();
            foreach (Job job in jobs)
            {
                for (int j = 0; j < clickcount; j++)
                {
                    if (job.length <= tables[j].length && tables[j].state == "空闲")
                    {
                        List<Table> temp = tables.ToList();
                        Table table = new Table();
                        table.address = tables[j].address + job.length;
                        table.length = tables[j].length - job.length;
                        table.state = "空闲";
                        table.occupier = "";
                        temp.Insert(j + 1, table);
                        tables = temp.ToArray();
                        allodata = new string[] { tables[j + 1].address.ToString(), tables[j + 1].length.ToString(), tables[j + 1].state, tables[j + 1].occupier };

                        tables[j].length = job.length;
                        tables[j].state = "被占用";
                        tables[j].occupier = job.name;
                        data = new string[] { tables[j].address.ToString(), tables[j].length.ToString(), tables[j].state, tables[j].occupier };
                        if (data.Length != 0)
                        {
                            ListViewItem lvi = new ListViewItem(data);
                            listView1.Items.RemoveAt(tables[j].index);
                            listView1.Items.Insert(tables[j].index+1, lvi);
                            lvi = new ListViewItem(allodata);
                            listView1.Items.Insert(tables[j].index+2, lvi);
                            listView2.Items.RemoveAt(0);
                            data = new string[] { };
                            allodata = new string[] { };
                        }
                        break;
                    }
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count > 0&&comboBox3.Text!="")
            {
                if (comboBox3.Text == "最先适应法")
                {
                    first_fit();
                }
                else if (comboBox3.Text == "最佳适应法")
                {
                    best_fit();
                }
                else if (comboBox3.Text == "最坏适应法")
                {
                    worst_fit();
                }
                if (listView2.Items.Count > 0)
                {
                    MessageBox.Show("剩余作业暂未找到合适的分区表");
                }
            }else if (comboBox3.Text == "")
            {
                MessageBox.Show("请先选择算法");
            }
            else
            {
                MessageBox.Show("请先添加作业再执行此操作");
            }
        }

        private void button6_Click(object sender, EventArgs e)      //动态内存分配
        {
            if (now_memory != 0)
            {
                tables[listView1.SelectedItems[0].Index].length += Convert.ToInt32(textBox1.Text);
                listView1.SelectedItems[0].SubItems[1].Text = tables[listView1.SelectedItems[0].Index].length.ToString();
                now_memory -= Convert.ToInt32(textBox1.Text);
                textBox3.Text = now_memory.ToString();
            }
            else
            {
                MessageBox.Show("已没有任何可分配的内存");
            }
        }

        private void button5_Click(object sender, EventArgs e)      //重置页面
        {
            tables = new Table[100];
            clickcount = 0;
            listView1.Items.Clear();
            listView2.Items.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
        }
    }
}
