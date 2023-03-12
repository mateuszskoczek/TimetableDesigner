using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TimetableDesigner.Core;

namespace TimetableDesigner.Tests
{
    [TestClass]
    public class TimetableTest
    {
        [TestMethod]
        public void CreateValidSlotTest()
        {
            TimetableSpan slot = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateInvalidSlotTest()
        {
            TimetableSpan slot = new TimetableSpan(new TimeOnly(9, 0), new TimeOnly(8, 0));

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateSlotWithZeroLengthTest()
        {
            TimetableSpan slot = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(8, 0));

            Assert.Fail();
        }

        [TestMethod]
        public void CreateModelTest()
        {
            TimetableTemplate model = new TimetableTemplate();
        }

        [TestMethod]
        public void AddDayTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableDay day = new TimetableDay
            {
                Name = "Monday"
            };

            model.AddDay(day);

            Assert.AreEqual(1, model.Days.Count());
            Assert.AreEqual(day, model.Days.ToList()[0]);
        }

        [TestMethod]
        public void AddSlotTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableSpan slot = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0));

            model.AddSlot(slot);

            Assert.AreEqual(1, model.Slots.Count());
            Assert.AreEqual(new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0)), model.Slots.ToList()[0]);
        }

        [TestMethod]
        public void AddNoCollidingSlotsTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableSpan slot1 = new TimetableSpan(new TimeOnly(8, 15), new TimeOnly(9, 0));
            TimetableSpan slot2 = new TimetableSpan(new TimeOnly(9, 15), new TimeOnly(10, 0));

            model.AddSlot(slot1);
            model.AddSlot(slot2);

            Assert.AreEqual(2, model.Slots.Count());
            Assert.AreEqual(new TimetableSpan(new TimeOnly(8, 15), new TimeOnly(9, 0)), model.Slots.ToList()[0]);
            Assert.AreEqual(new TimetableSpan(new TimeOnly(9, 15), new TimeOnly(10, 0)), model.Slots.ToList()[1]);
        }

        [TestMethod]
        public void AddSlotsWithoutBreakTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableSpan slot1 = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0));
            TimetableSpan slot2 = new TimetableSpan(new TimeOnly(9, 0), new TimeOnly(10, 0));

            model.AddSlot(slot1);
            model.AddSlot(slot2);

            Assert.AreEqual(2, model.Slots.Count());
            Assert.AreEqual(new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0)), model.Slots.ToList()[0]);
            Assert.AreEqual(new TimetableSpan(new TimeOnly(9, 0), new TimeOnly(10, 0)), model.Slots.ToList()[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCollidingSlotsTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableSpan slot1 = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 30));
            TimetableSpan slot2 = new TimetableSpan(new TimeOnly(8, 30), new TimeOnly(10, 0));

            model.AddSlot(slot1);
            model.AddSlot(slot2);

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBetweenCollidingSlotsTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableSpan slot1 = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0));
            TimetableSpan slot2 = new TimetableSpan(new TimeOnly(10, 0), new TimeOnly(11, 0));
            TimetableSpan slot3 = new TimetableSpan(new TimeOnly(8, 59), new TimeOnly(10, 1));

            model.AddSlot(slot1);
            model.AddSlot(slot2);
            model.AddSlot(slot3);

            Assert.Fail();
        }

        [TestMethod]
        public void SortSlotsTest()
        {
            TimetableTemplate model = new TimetableTemplate();

            TimetableSpan slot1 = new TimetableSpan(new TimeOnly(12, 0), new TimeOnly(13, 0));
            TimetableSpan slot2 = new TimetableSpan(new TimeOnly(8, 0), new TimeOnly(9, 0));
            TimetableSpan slot3 = new TimetableSpan(new TimeOnly(10, 0), new TimeOnly(11, 0));
            TimetableSpan slot4 = new TimetableSpan(new TimeOnly(14, 0), new TimeOnly(15, 0));
            TimetableSpan slot5 = new TimetableSpan(new TimeOnly(13, 0), new TimeOnly(14, 0));
            TimetableSpan slot6 = new TimetableSpan(new TimeOnly(9, 0), new TimeOnly(10, 0));
            TimetableSpan slot7 = new TimetableSpan(new TimeOnly(11, 0), new TimeOnly(12, 0));

            model.AddSlot(slot1);
            model.AddSlot(slot2);
            model.AddSlot(slot3);
            model.AddSlot(slot4);
            model.AddSlot(slot5);
            model.AddSlot(slot6);
            model.AddSlot(slot7);

            List<TimetableSpan> slots = model.Slots.ToList();

            TimetableSpan testSlot = slots[0];
            for (int i = 1; i < slots.Count; i++)
            {
                if (testSlot.To > slots[i].From)
                {
                    Assert.Fail();
                }
                testSlot = slots[i];
            }
        }
    }
}
