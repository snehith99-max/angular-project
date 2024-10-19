import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyleadsaddleadComponent } from './crm-trn-myleadsaddlead.component';

describe('CrmTrnMyleadsaddleadComponent', () => {
  let component: CrmTrnMyleadsaddleadComponent;
  let fixture: ComponentFixture<CrmTrnMyleadsaddleadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyleadsaddleadComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyleadsaddleadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
