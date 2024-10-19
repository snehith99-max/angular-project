import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyvisitexpiredComponent } from './crm-trn-myvisitexpired.component';

describe('CrmTrnMyvisitexpiredComponent', () => {
  let component: CrmTrnMyvisitexpiredComponent;
  let fixture: ComponentFixture<CrmTrnMyvisitexpiredComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyvisitexpiredComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyvisitexpiredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
