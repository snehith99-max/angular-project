import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesorderviewNewComponent } from './smr-trn-salesorderview-new.component';

describe('SmrTrnSalesorderviewNewComponent', () => {
  let component: SmrTrnSalesorderviewNewComponent;
  let fixture: ComponentFixture<SmrTrnSalesorderviewNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesorderviewNewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesorderviewNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
