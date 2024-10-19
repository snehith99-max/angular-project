import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnJournalentryAddComponent } from './acc-trn-journalentry-add.component';

describe('AccTrnJournalentryAddComponent', () => {
  let component: AccTrnJournalentryAddComponent;
  let fixture: ComponentFixture<AccTrnJournalentryAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnJournalentryAddComponent]
    });
    fixture = TestBed.createComponent(AccTrnJournalentryAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
