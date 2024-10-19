import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstCustomereportalpreviewmailComponent } from './smr-mst-customereportalpreviewmail.component';

describe('SmrMstCustomereportalpreviewmailComponent', () => {
  let component: SmrMstCustomereportalpreviewmailComponent;
  let fixture: ComponentFixture<SmrMstCustomereportalpreviewmailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstCustomereportalpreviewmailComponent]
    });
    fixture = TestBed.createComponent(SmrMstCustomereportalpreviewmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
