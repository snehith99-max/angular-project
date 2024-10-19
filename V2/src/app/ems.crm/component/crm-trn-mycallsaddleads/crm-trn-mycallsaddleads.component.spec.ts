import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMycallsaddleadsComponent } from './crm-trn-mycallsaddleads.component';

describe('CrmTrnMycallsaddleadsComponent', () => {
  let component: CrmTrnMycallsaddleadsComponent;
  let fixture: ComponentFixture<CrmTrnMycallsaddleadsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMycallsaddleadsComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMycallsaddleadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
